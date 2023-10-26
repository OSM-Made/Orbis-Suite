#include "stdafx.h"
#include "Flash.h"
#include "Target.h"
#include <NetExt.h>
#include <user_service.h>
#include "ShellCoreUtil.h"
#include "UserServiceExt.h"
#include "SystemServiceExt.h"
#include "Debug.h"

void Target::SendTargetInfo(SceNetId sock)
{
	TargetInfoPacket packet;

	packet.set_sdkversion(GetSDKVersion());
	packet.set_softwareversion(GetUpdateVersion());
	//ReadFlash(FLASH_FACTORY_FW, &Packet->FactorySoftwareVersion, sizeof(int));
	packet.set_cputemp(GetCPUTemp());
	packet.set_soctemp(GetSOCTemp());

	// Current Big App.
	auto bigApp = packet.mutable_bigapp();
	auto bigAppAppId = sceSystemServiceGetAppIdOfBigApp();
	if (bigAppAppId > 0)
	{
		// Get the list of running processes.
		std::vector<kinfo_proc> processList;
		GetProcessList(processList);

		for (const auto& i : processList)
		{
			// Get the app info using the pid.
			SceAppInfo appInfo;
			sceKernelGetAppInfo(i.pid, &appInfo);

			// Using the titleId match our desired app and return the appId from the appinfo.
			if (appInfo.AppId == bigAppAppId)
			{
				bigApp->set_pid(i.pid);
				bigApp->set_name(i.name);
				bigApp->set_titleid(appInfo.TitleId);

				break;
			}
		}
	}
	else
	{
		bigApp->set_titleid("N/A");
	}

	packet.set_consolename(GetConsoleName().c_str());

	// TODO: Convert these to be string value.
	//ReadFlash(FLASH_MB_SERIAL, &Packet->MotherboardSerial, 14);
	//ReadFlash(FLASH_SERIAL, &Packet->Serial, 10);
	//ReadFlash(FLASH_MODEL, &Packet->Model, 14);
	packet.set_macaddresslan(GetMacAddress(SCE_NET_IF_NAME_ETH0).c_str());
	packet.set_macaddresswifi(GetMacAddress(SCE_NET_IF_NAME_WLAN0).c_str());
	
	// TODO: Flash revamp.
	//ReadFlash(FLASH_UART_FLAG, &Packet->UART, 1);
	//ReadFlash(FLASH_IDU_MODE, &Packet->IDUMode, 1);

	packet.set_idps(GetIdPs());
	packet.set_psid(GetPsId());
	packet.set_consoletype(GetConsoleType());

	// Debugging.
	packet.set_attachedpid(Debug::CurrentPID);
	packet.set_attached(Debug::IsDebugging);

	// User.
	packet.set_foregroundaccountid(GetForeGroundUserId());

	// Storage Stats.
	auto storageStats = GetStorageStats();
	packet.set_freespace(std::get<0>(storageStats));
	packet.set_totalspace(std::get<1>(storageStats));

	// Perf Stats. TODO: Move from toolbox
	/*Packet->ThreadCount = SystemMonitor::Thread_Count;
	Packet->AverageCPUUsage = SystemMonitor::Average_Usage;
	Packet->BusyCore = SystemMonitor::Busy_Core;
	memcpy(&Packet->Ram, &SystemMonitor::RAM, sizeof(MemoryInfo));
	memcpy(&Packet->VRam, &SystemMonitor::VRAM, sizeof(MemoryInfo));*/

	SendProtobufPacket(sock, packet);
}

void Target::DoNotify(SceNetId sock)
{
	TargetNotifyPacket packet;
	if (!RecieveProtoBuf<TargetNotifyPacket>(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	if (packet.iconuri() == "")
		Notify(packet.message().c_str());
	else
		NotifyCustom(packet.iconuri().c_str(), packet.message().c_str());

	// Send the happy state.
	SendStatePacket(sock, true, "");
}

void Target::DoBuzzer(SceNetId sock)
{
	BuzzerType buzzerType = RingOnce;
	if (Sockets::RecvInt(sock, (int*)&buzzerType))
	{
		RingBuzzer(buzzerType);
	}
}

void Target::SetConsoleLED(SceNetId sock)
{
	ConsoleLEDColours ledColour = white;
	if (Sockets::RecvInt(sock, (int*)&ledColour))
	{
		SetConsoleLED(ledColour);
	}
}

void Target::SetSettings(SceNetId sock)
{
	//TODO: IPC here...
	/*auto Packet = new TargetSettingsPacket();

	sceNetRecv(Sock, Packet, sizeof(TargetSettingsPacket), 0);

	Debug_Feature::DebugTitleIdLabel::ShowLabels = Config::Data->Show_DebugTitleIdLabel = Packet->ShowDebugTitleIdLabel;
	Debug_Feature::DevkitPanel::ShowPanel = Config::Data->Show_DevkitPanel = Packet->ShowDevkitPanel;
	Debug_Feature::Custom_Content::Show_Debug_Settings = Config::Data->Show_Debug_Settings = Packet->ShowDebugSettings;
	Debug_Feature::Custom_Content::Show_App_Home = Config::Data->Show_App_Home = Packet->ShowAppHome;

	SendStatus(Sock, APIResults::API_OK);

	Config::SetSettingsNow = true;*/
}

void Target::ProcList(SceNetId sock)
{
	// Get the list of running processes.
	std::vector<kinfo_proc> processList;
	GetProcessList(processList);

	std::vector<ProcPacket> vectorList;
	for (const auto& i : processList)
	{
		// Get the app info using the pid.
		SceAppInfo appInfo;
		sceKernelGetAppInfo(i.pid, &appInfo);

		// Build packet.
		ProcPacket procPacket;
		procPacket.set_appid(appInfo.AppId);
		procPacket.set_processid(i.pid);
		procPacket.set_name(i.name);
		procPacket.set_titleid(appInfo.TitleId);
		vectorList.push_back(procPacket);
	}

	// Set the parsed list into the protobuf packet.
	ProcListPacket packet;
	*packet.mutable_processes() = { vectorList.begin(), vectorList.end() };

	// Send the list to host.
	SendProtobufPacket(sock, packet);
}

void Target::SendFile(SceNetId sock)
{
	FilePacket packet;
	if (!RecieveProtoBuf<FilePacket>(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Open file descriptors 
	auto fd = sceKernelOpen(packet.filepath().c_str(), SCE_KERNEL_O_RDONLY, 0);
	if (fd <= 0)
	{
		SendStatePacket(sock, false, "Failed to open the file \"%s\".", packet.filepath().c_str());
		Logger::Error("Failed to open file \"%s\".\n", packet.filepath().c_str());
		return;
	}

	// Get File size
	SceKernelStat stats;
	sceKernelFstat(fd, &stats);

	if (stats.st_size == 0)
	{
		SendStatePacket(sock, false, "Failed to get size of the file \"%s\".", packet.filepath().c_str());
		Logger::Error("Failed to get size of file \"%s\"..\n", packet.filepath().c_str());
		return;
	}

	// Allocate space to read data.
	auto FileData = (unsigned char*)malloc(stats.st_size);

	// ReadFile.
	sceKernelRead(fd, FileData, stats.st_size);
	sceKernelClose(fd);

	// Send the happy packet.
	SendStatePacket(sock, true, "");

	Sockets::SendInt(sock, stats.st_size);
	Sockets::SendLargeData(sock, FileData, stats.st_size);

	free(FileData);
}

void Target::RecieveFile(SceNetId sock)
{
	FilePacket packet;
	if (!RecieveProtoBuf<FilePacket>(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Open file descriptors 
	auto fd = sceKernelOpen(packet.filepath().c_str(), SCE_KERNEL_O_CREAT | SCE_KERNEL_O_RDWR, 0777);
	if (fd <= 0)
	{
		SendStatePacket(sock, false, "Failed to open the file \"%s\" with error 0x%llX.", packet.filepath().c_str(), fd);
		Logger::Error("Failed to open file \"%s\" with error 0x%llX.\n", packet.filepath().c_str(), fd);
		return;
	}

	// Send the happy packet.
	SendStatePacket(sock, true, "");

	int fileSize;
	if (!Sockets::RecvInt(sock, &fileSize))
	{
		Logger::Error("Failed to get the file size.\n");
		return;
	}

	// Allocate space to read data.
	auto fileData = (unsigned char*)malloc(fileSize);

	if (!Sockets::RecvLargeData(sock, fileData, fileSize))
	{
		Logger::Error("Failed to get the file data.\n");
		return;
	}

	sceKernelWrite(fd, fileData, fileSize);
	sceKernelClose(fd);

	free(fileData);
}

void Target::DeleteFile(SceNetId sock)
{
	FilePacket packet;
	if (!RecieveProtoBuf<FilePacket>(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	auto res = sceKernelUnlink(packet.filepath().c_str());
	if (res != 0)
	{
		Logger::Error("Failed to delete the file \"%s\" 0x%llX.\n", packet.filepath().c_str(), res);
		SendStatePacket(sock, false, "Failed to delete the file \"%s\" 0x%llX.\n", packet.filepath().c_str(), res);
		return;
	}

	SendStatePacket(sock, true, "");
}