#include "stdafx.h"
#include <7z/7zExtractor.h>
#include <AppControl.h>

#define DEBUG

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
	mount_large_fs("/dev/da0x4.crypt", "/system", "exfatfs", "511", MNT_UPDATE);
	
	// Install all the things! :D
	Logger::Info("Extracting OrbisLib Deamon.\n");
	Extract7zFile("/mnt/sandbox/ORBS00000_000/app0/Daemons/ORBS30000.7z", "/system/vsh/app/");

	Logger::Info("Extracting Orbis Toolbox.\n");
	Extract7zFile("/mnt/sandbox/ORBS00000_000/app0/Orbis Toolbox.7z", "/data/");

	Logger::Info("Making Orbis Suite Directory\n");
	MakeDir("/data/Orbis Suite");

	// Launch the daemon for everyone.
	Logger::Info("Starting or Restarting OrbisLib Deamon.\n");
	auto res = StartRestartApp("ORBS30000", nullptr, SCE_USER_SERVICE_USER_ID_EVERYONE);

	if (res != 0)
	{
		Notify("Failed to start the OrbisLib Daemon. :(");
	}

	ExitGraceful();
	return 0;
}