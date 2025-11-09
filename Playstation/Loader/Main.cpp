#include "stdafx.h"

bool LoadModules()
{
	auto res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_SYSTEM_SERVICE);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_SYSTEM_SERVICE (%llX)\n", res);
		return false;
	}

	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_USER_SERVICE);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_USER_SERVICE (%llX)\n", res);
		return false;
	}

	SceUserServiceInitializeParams userParam = { SCE_KERNEL_PRIO_FIFO_HIGHEST };
	res = sceUserServiceInitialize(&userParam);
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceUserServiceInitialize failed (%llX)\n", res);
		return false;
	}

	res = sceLncUtilInitialize();
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceLncUtilInitialize failed (%llX)\n", res);
		return false;
	}

	Logger::Success("LoadModules(): Success!\n");
	return true;
}

int main(int argc, char** arg)
{
	Logger::Init(true, Logger::LogLevelAll);

	Logger::Info("Hello from OrbisLib Loader\n");

	// Jailbreak our current process.
	Logger::Info("Jailbreaking our process.\n");
	if (!Jailbreak())
	{
		Notify("Failed to jailbreak Process...");
		ExitGraceful();
		return 0;
	}

	// Load internal system modules.
	Logger::Info("Loading modules.\n");
	if (!LoadModules())
	{
		Notify("Failed to Load Modules...");
		ExitGraceful();
		return 0;
	}

	// Set RW on the system directory.
	Logger::Info("Mounting System as R/W.\n");
	RemountReadWrite("/dev/da0x4.crypt", "/system");
	
	Logger::Info("Extracting OrbisLib Deamon.\n");
	Extract7zFile("/mnt/sandbox/ORBS00000_000/app0/ORBS30000.7z", "/system/vsh/app/");

	Logger::Info("Making Orbis Suite Directory\n");
	FileSystem::MakeDir("/data/Orbis Suite");
	 
	Logger::Info("Starting or Restarting OrbisLib Deamon.\n");
	auto res = StartRestartApp("ORBS30000", nullptr, SCE_USER_SERVICE_USER_ID_EVERYONE);
	 
	if (res < 0)
	{
		Notify("Failed to start the OrbisLib Daemon. :(");
	}

	ExitGraceful();
	return 0;
}