#include "stdafx.h"
#include "Utilities.h"

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

bool Jailbreak()
{
	jailbreak_backup bk;
	return (sys_sdk_jailbreak(&bk) == 0);
}

void InstallDaemon(const char* Daemon, const char* libs[], int libCount)
{
	Logger::Info("Installing Daemon %s...\n[%s] Making Directories...\n", Daemon, Daemon);
	MakeDir("/system/vsh/app/%s", Daemon);
	MakeDir("/system/vsh/app/%s/sce_sys", Daemon);

	Logger::Info("[%s] Writing Files...\n", Daemon);
	char EbootFromPath[0x100], EbootPath[0x100];
	sprintf(EbootFromPath, "%s%s/eboot.bin", DAEMONGETPATH, Daemon);
	sprintf(EbootPath, "%s%s/eboot.bin", DAEMONPATH, Daemon);
	CopyFile(EbootFromPath, EbootPath);

	char ParamFromPath[0x100], ParamPath[0x100];
	sprintf(ParamFromPath, "%s%s/sce_sys/param", DAEMONGETPATH, Daemon);
	sprintf(ParamPath, "%s%s/sce_sys/param.sfo", DAEMONPATH, Daemon);
	CopyFile(ParamFromPath, ParamPath);

	char IconFromPath[0x100], IconPath[0x100];
	sprintf(IconFromPath, "%s%s/sce_sys/icon0.png", DAEMONGETPATH, Daemon);
	sprintf(IconPath, "%s%s/sce_sys/icon0.png", DAEMONPATH, Daemon);
	CopyFile(IconFromPath, IconPath);

	for (int i = 0; i < libCount; i++)
	{
		Logger::Info("Copying Library %s\n", libs[i]);
		char LibraryFromPath[0x100], LibraryPath[0x100];
		sprintf(LibraryFromPath, "%s%s/%s", DAEMONGETPATH, Daemon, libs[i]);
		sprintf(LibraryPath, "%s%s/%s", DAEMONPATH, Daemon, libs[i]);
		CopyFile(LibraryFromPath, LibraryPath);
	}

	Logger::Success("[%s] Installation Success!\n", Daemon);
}

void InstallOrbisToolbox()
{
	const char* Icons[] = { "icon_daemon.png", "icon_payload.png", "icon_pkg.png", "icon_plugin.png", "icon_reboot.png", "icon_reload_ui.png", "icon_shutdown.png", "icon_suspend.png", "icon_system_settings.png", "icon_toolbox.png" };

	Logger::Info("[Orbis Toolbox] Making Directories...\n");
	MakeDir("/data/Orbis Toolbox");
	MakeDir("/data/Orbis Toolbox/Plugins");
	MakeDir("/data/Orbis Toolbox/Icons");
	MakeDir("/data/Orbis Toolbox/Payloads");

	Logger::Info("[Orbis Toolbox] Writing Files...\n");
	CopyFile("/mnt/sandbox/ORBS00000_000/app0/Orbis Toolbox/OrbisToolbox-2.0.sprx", "/data/Orbis Toolbox/OrbisToolbox-2.0.sprx");
	for (int i = 0; i < 10; i++)
	{
		char IconFromPath[0x100], IconPath[0x100];
		sprintf(IconFromPath, ICONGETPATH, Icons[i]);
		sprintf(IconPath, ICONPATH, Icons[i]);
		CopyFile(IconFromPath, IconPath);
	}

	Logger::Success("[Orbis Toolbox] Installation Success!\n");
}

void InstallOrbisSuite()
{
	Logger::Info("[Orbis Suite] Making Directories...\n");
	MakeDir("/data/Orbis Suite");
	MakeDir("/data/Orbis Suite/IPC");
}