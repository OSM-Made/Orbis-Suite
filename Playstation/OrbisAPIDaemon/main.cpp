#include "stdafx.h"
#include "API.h"
#include "SystemMonitor.h"
#include "ProcessMonitor.h"
#include <SysCoreUtil.h>
#include <SystemInterface.h>
#include <mdbg.h>

int main(int argc, char** arg)
{
	// Set up the Logger.
	Logger::Init(true, Logger::LoggingLevels::LogLevelAll);

	// Jailbreak our current process.
	if (!Jailbreak(0x3800000000010003, true))
	{
		Notify("Failed to jailbreak Process...");
		ExitGraceful();
		return 0;
	}

	// Load internal system modules.
	if (!LoadModules())
	{
		Notify("Failed to Load Modules...");
		ExitGraceful();
		return 0;
	}

	auto res1 = sceKernelLoadStartModule("/system/priv/lib/libmdbg_syscore.sprx", 0, 0, 0, 0, 0);
	if (res1 < 0)
	{
		Logger::Error("LoadModules(): Failed to load libmdbg_syscore.sprx (%llX)\n", res1);
		ExitGraceful();
		return 0;
	}

	auto res = sceDebugInit();
	Logger::Info("sceDebugInit: %x\n", res);
	
	auto pid = GetPidByName("SceShellUI");
	Logger::Info("SceShellUI pid: %x\n", pid);
	
	res = sceDebugAttachProcess(pid);
	Logger::Info("sceDebugAttachProcess: %x\n", res);
	
	res = sceDebugResumeProcess(pid);
	Logger::Info("sceDebugResumeProcess: %x\n", res);
	
	res = sceDebugDetachProcess(pid);
	Logger::Info("sceDebugDetachProcess: %x\n", res);

	// Load the toolbox.
	//LoadToolbox();

	// Copy back up of sflash so we can read it and not break things :)
	//CopySflash();
	//
	//// Set the Name of this process so it shows up as something other than eboot.bin.
	//sceKernelSetProcessName("OrbisAPIDaemon");
	//
	//// Start up the thread pool.
	//ThreadPool::Init(10);
	//
	//// Log the loaded version string.
	//Logger::Info("%s\n", ORBISLIB_BUILDSTRING);
	//
	//// Start up the API.
	//API::Init();
	//
	//// Blocking run the system monitor.
	//SystemMonitor::Run();

	ExitGraceful();
	return 0;
}