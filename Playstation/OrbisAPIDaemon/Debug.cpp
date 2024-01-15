#include "stdafx.h"
#include "ProcessMonitor.h"
#include "Debug.h"
#include "Events.h"
#include "PtraceDefs.h"
#include "SignalDefs.h"
#include <FusionDriver.h>

std::mutex Debug::DebugMtx;
bool Debug::IsDebugging;
int Debug::CurrentPID;
std::shared_ptr<ProcessMonitor> Debug::DebuggeeMonitor;

bool Debug::CheckDebug(SceNetId s)
{
	if (!IsDebugging || CurrentPID == -1)
	{
		Sockets::SendInt(s, 0);
		return false;
	}

	Sockets::SendInt(s, 1);
	return true;
}

void Debug::Attach(SceNetId sock)
{
	auto pid = 0;
	if (!Sockets::RecvInt(sock, &pid))
	{
		Logger::Error("Attach(): Failed to recieve the pid\n");
		SendStatePacket(sock, false, "Failed to recieve the pid.");
		return;
	}

	// Get Process name.
	char processName[32];
	sceKernelGetProcessName(pid, processName);

	{
		std::unique_lock<std::mutex> lock(DebugMtx);

		Logger::Info("Attach(): Attempting to attach to %s (%d)\n", processName, pid);

		// If we are currently debugging another process lets detach from it.
		//if (!TryDetach(pid))
		//{
		//	Logger::Error("Attach(): TryDetach Failed. :(\n");
		//	SendStatePacket(sock, false, "Try detach failed.");
		//	return;
		//}
		//
		//// Use ptrace to attach to begin debugging this pid.
		//int res = ptrace(PT_ATTACH, pid, nullptr, 0);
		//if (res != 0)
		//{
		//	Logger::Error("Attach(): ptrace(PT_ATTACH) failed with error %llX %s\n", __error(), strerror(errno));
		//	SendStatePacket(sock, false, "Attach failed: %llX %s", __error(), strerror(errno));
		//	return;
		//}
		//
		//// Wait till the process haults.
		//waitpid(pid, NULL, 0);
		//
		//// Attaching by default will stop execution of the remote process. Lets continue it now.
		//res = ptrace(PT_CONTINUE, pid, (void*)1, 0);
		//if (res != 0)
		//{
		//	Logger::Error("Attach(): ptrace(PT_CONTINUE) failed with error %llX %s\n", __error(), strerror(errno));
		//	SendStatePacket(sock, false, "Continue failed: %llX %s", __error(), strerror(errno));
		//	return;
		//}

		// Set current debugging state.
		IsDebugging = true;
		CurrentPID = pid;

		// Set up proc monitor.
		DebuggeeMonitor = std::make_shared<ProcessMonitor>(pid);
		DebuggeeMonitor->OnExit = OnExit; // Fired when a process dies.
		DebuggeeMonitor->OnException = OnException; // Fired when the process being debugged encounters an excepton.
	}

	// Send attach event to host.
	Events::SendEvent(Events::EVENT_ATTACH, pid);

	Logger::Info("Attach(): Attached to %s(%d)\n", processName, pid);

	// Send the happy state.
	SendStatePacket(sock, true, "");

	// Mount /data/ in sandbox.
	if (strcmp(processName, "SceShellCore"))
	{
		// Get app info.
		SceAppInfo appInfo;
		sceKernelGetAppInfo(pid, &appInfo);

		// Mount data & system into sandbox
		LinkDir("/data/", va("/mnt/sandbox/%s_000/data", appInfo.TitleId).c_str());
		LinkDir("/system/", va("/mnt/sandbox/%s_000/system", appInfo.TitleId).c_str());
	}
}

void Debug::Detach(SceNetId sock)
{
	if (!IsDebugging)
		Sockets::SendInt(sock, 0);

	{
		std::unique_lock<std::mutex> lock(DebugMtx);

		// Get app info.
		SceAppInfo appInfo;
		sceKernelGetAppInfo(CurrentPID, &appInfo);

		// Unmount the linked dirs.
		unmount(va("/mnt/sandbox/%s_000/data", appInfo.TitleId).c_str(), MNT_FORCE);
		unmount(va("/mnt/sandbox/%s_000/system", appInfo.TitleId).c_str(), MNT_FORCE);

		//if (TryDetach(CurrentPID))
		{
			// Reset vars.
			IsDebugging = false;
			CurrentPID = -1;

			Events::SendEvent(Events::EVENT_DETACH);
			SendStatePacket(sock, true, "");
		}
		//else
		//{
		//	Logger::Error("Failed to detach from %d\n", CurrentPID);
		//	SendStatePacket(sock, false, "Failed to detach from %d", CurrentPID);
		//}
	}
}

void Debug::Current(SceNetId sock)
{
	if (!IsDebugging)
	{
		Sockets::SendInt(sock, -1);
	}
	else
	{
		Sockets::SendInt(sock, CurrentPID);
	}
}

void Debug::RWMemory(SceNetId s, bool write)
{
	if (!CheckDebug(s))
		return;

	RWPacket packet;
	if (!RecieveProtoBuf<RWPacket>(s, &packet))
	{
		SendStatePacket(s, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Allocate space for our read / write.
	std::unique_ptr<unsigned char[]> buffer;
	try
	{
		
		buffer = std::make_unique<unsigned char[]>(packet.length());
	}
	catch (const std::exception& ex)
	{
		SendStatePacket(s, false, "Failed to allocate enough memory.");
		return;
	}

	// TODO: Might be a good idea to make sure we are landing in the right memory regions. Should be good to check the vmmap and the library list.
	//		 Pretty sure we can use the syscall from the kernel context and specify the debug proc to achieve the same. 
	//		 (syscall 572) sceKernelVirtualQuery(const void* address, int flags, SceKernelVirtualQueryInfo* info, size_t infoSize)

	if (write)
	{
		// Send happy packet so we can continue on.
		SendStatePacket(s, true, "");

		// Recieve the data we are going to write.
		if (!Sockets::RecvLargeData(s, buffer.get(), packet.length()))
		{
			Logger::Error("Debug::RWMemory(): Failed to recieve memory to write\n");
			SendStatePacket(s, false, " Failed to recieve memory to write.");
			return;
		}

		// Write the memory we recieved using the kernel.
		auto res = Fusion::ReadWriteMemory(CurrentPID, packet.address(), (void*)buffer.get(), packet.length(), true);
		if (res != 0)
		{
			Logger::Error("Debug::RWMemory(): Failed to write memory to process %i at %llX for reason %llX.\n", CurrentPID, packet.address(), res);
			SendStatePacket(s, false, "Failed to write memory to process %i at %llX for reason %llX.", CurrentPID, packet.address(), res);
			return;
		}

		// Send happy packet
		SendStatePacket(s, true, "");
	}
	else
	{
		// Read the memory requested using the kernel.
		auto res = Fusion::ReadWriteMemory(CurrentPID, packet.address(), (void*)buffer.get(), packet.length(), false);
		if (res != 0)
		{
			Logger::Error("Debug::RWMemory(): Failed to read memory from process %i at %llX for reason %llX.\n", CurrentPID, packet.address(), res);
			SendStatePacket(s, false, "Failed to read memory from process %i at %llX for reason %llX.", CurrentPID, packet.address(), res);
			return;
		}

		// Send happy packet
		SendStatePacket(s, true, "");

		// Send the data we read.
		if (!Sockets::SendLargeData(s, buffer.get(), packet.length()))
		{
			Logger::Error("Failed to send memory\n");
			return;
		}
	}
}

void Debug::OnExit()
{
	Logger::Info("Process %d has died!\n", CurrentPID);

	// Send the event to the host that the process has died.
	Events::SendEvent(Events::EVENT_DIE, CurrentPID);

	// Get app info.
	SceAppInfo appInfo;
	sceKernelGetAppInfo(CurrentPID, &appInfo);

	// Unmount the linked dirs.
	unmount(va("/mnt/sandbox/%s_000/data", appInfo.TitleId).c_str(), MNT_FORCE);
	unmount(va("/mnt/sandbox/%s_000/system", appInfo.TitleId).c_str(), MNT_FORCE);

	// For now just detach.
	if (!TryDetach(CurrentPID))
	{
		Logger::Error("OnExit(): TryDetach Failed. :(\n");
		return;
	}

	Events::SendEvent(Events::EVENT_DETACH);
}

void Debug::OnException(int status)
{
	int signal = WSTOPSIG(status);

	switch (signal)
	{
	case SIGSTOP:
		Logger::Info("SIGSTOP\n");
		break;
	}

	// Get app info.
	SceAppInfo appInfo;
	sceKernelGetAppInfo(CurrentPID, &appInfo);

	// Unmount the linked dirs.
	unmount(va("/mnt/sandbox/%s_000/data", appInfo.TitleId).c_str(), MNT_FORCE);
	unmount(va("/mnt/sandbox/%s_000/system", appInfo.TitleId).c_str(), MNT_FORCE);

	// For now just detach.
	if (!TryDetach(CurrentPID))
	{
		Logger::Error("OnException(): TryDetach Failed. :(\n");
		return;
	}

	Events::SendEvent(Events::EVENT_DETACH);
}

bool Debug::TryDetach(int pid)
{
	// Check if we are even attached.
	if (!IsDebugging)
	{
		return true;
	}

	// Detach from the process.
	int res = ptrace(PT_DETACH, pid, nullptr, 0);
	if (res != 0)
	{
		// Check if proc is dead anyway and just detach.
		std::vector<kinfo_proc> procList;
		GetProcessList(procList);

		if (std::find_if(procList.begin(), procList.end(), [=](const kinfo_proc& arg) { return arg.pid == pid; }) == procList.end())
		{
			// Reset vars.
			IsDebugging = false;
			CurrentPID = -1;

			return true;
		}

		Logger::Error("DetachProcess(): ptrace(PT_DETACH) failed with error %llX %s\n", __error(), strerror(errno));
		return false;
	}

	// Reset vars.
	IsDebugging = false;
	CurrentPID = -1;

	// Kill the current proc monitor.
	DebuggeeMonitor.reset();

	return true;
}