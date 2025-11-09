#include "stdafx.h"
#include "API.h"
#include "SystemMonitor.h"
#include "ProcessMonitor.h"
#include <SysCoreUtil.h>
#include <SystemInterface.h>
#include <mdbg.h>
#include <KernelExt.h>
#include <Dipsw.h>
#include <NetExt.h>

int main(int argc, char** arg)
{
	// Set up the Logger.
	Logger::Init(true, Logger::LoggingLevels::LogLevelAll);

	// Jailbreak our current process.
	if (!Jailbreak(0x3800000000010003))
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

	// Set the Name of this process so it shows up as something other than eboot.bin.
	sceKernelSetProcessName("OrbisAPIDaemon");
	
	// Start up the thread pool.
	ThreadPool::Init(10);
	
	// Log the loaded version string.
	Logger::Info("%s\n", ORBISLIB_BUILDSTRING);
	
	// Start up the API.
	API::Init();
	
	// Blocking run the system monitor.
	SystemMonitor::Run();

	ExitGraceful();
	return 0;
}