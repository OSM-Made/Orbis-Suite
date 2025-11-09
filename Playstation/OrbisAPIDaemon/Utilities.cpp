#include "stdafx.h"
#include <NetExt.h>
#include "Utilities.h"
#include <libnetctl.h>
#include <FusionDriver.h>
#include <SystemInterface.h>
#include <mdbg.h>

bool LoadModules()
{
	auto res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_SYSTEM_SERVICE);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_SYSTEM_SERVICE (%llX)\n", res);
		return false;
	}
	
	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_APPINSTUTIL);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_APPINSTUTIL (%llX)\n", res);
		return false;
	}
	
	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_USER_SERVICE);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_USER_SERVICE (%llX)\n", res);
		return false;
	}
	
	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_SYS_CORE);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_SYS_CORE (%llX)\n", res);
		return false;
	}
	
	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_NETCTL);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_NETCTL (%llX)\n", res);
		return false;
	}
	
	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_NET);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_NET (%llX)\n", res);
		return false;
	}
	
	res = sceSysmoduleLoadModuleInternal(SCE_SYSMODULE_INTERNAL_BGFT);
	if (res != 0)
	{
		Logger::Error("LoadModules(): Failed to load SCE_SYSMODULE_INTERNAL_BGFT (%llX)\n", res);
		return false;
	}

	res = sceKernelLoadStartModule("/system/priv/lib/libSceDipsw.sprx", 0, 0, 0, 0, 0);
	if (res < 0)
	{
		Logger::Error("LoadModules(): Failed to load libSceDipsw.sprx (%llX)\n", res);
		return false;
	}

	// res = sceKernelLoadStartModule("/system/priv/lib/libmdbg_syscore.sprx", 0, 0, 0, 0, 0);
	// if (res < 0)
	// {
	// 	Logger::Error("LoadModules(): Failed to load libmdbg_syscore.sprx (%llX)\n", res);
	// 	ExitGraceful();
	// 	return 0;
	// }
	// 
	// // Start debug.
	// res = sceDebugInit();
	// if (res != 0)
	// {
	// 	Logger::Error("LoadModules(): sceDebugInit failed (%llX)\n", res);
	// 	return false;
	// }

	// Start up networking interface
	res = sceNetInit();
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceNetInit failed\n");
		return false;
	}

	// Start up user service.
	SceUserServiceInitializeParams userParam = { SCE_KERNEL_PRIO_FIFO_HIGHEST };
	res = sceUserServiceInitialize(&userParam);
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceUserServiceInitialize failed (%llX)\n", res);
		return false;
	}

	// Init temporary wrapper for lncutils.
	res = sceLncUtilInitialize();
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceLncUtilInitialize failed (%llX)\n", res);
		return false;
	}

	// Init SysCoreUtils.
	res = sceApplicationInitialize();
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceApplicationInitialize failed (%llX)\n", res);
		return false;
	}

	// Init App install utils.
	res = sceAppInstUtilInitialize();
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceAppInstUtilInitialize failed (%llX)\n", res);
		return false;
	}

	res = sceNetCtlInit();
	if (res != 0)
	{
		Logger::Error("LoadModules(): sceNetCtlInit failed (%llX)\n", res);
		return false;
	}

	Logger::Success("LoadModules(): Success!\n");
	return true;
}

bool LoadSymbol(SceKernelModule handle, const char* symbol, void** funcOut)
{
	if (sceKernelDlsym(handle, symbol, funcOut) != 0 || *funcOut == 0)
	{
		Logger::Error("Failed to load %s.\n", symbol);
		return false;
	}

	return true;
}

bool CopySflash()
{
	int sflashFd = sceKernelOpen("/dev/sflash0", SCE_KERNEL_O_RDONLY, 0);
	int backupFd = sceKernelOpen("/data/Orbis Suite/sflash0", SCE_KERNEL_O_CREAT | SCE_KERNEL_O_WRONLY | SCE_KERNEL_O_APPEND, 0777);
	if (sflashFd && backupFd)
	{
		auto buffer = (unsigned char*)malloc(4 * 1024 * 1024);
		if (buffer == nullptr)
		{
			Logger::Error("failled to allocate memory for sflash read.\n");
			return false;
		}

		size_t bytesRead = 0;
		while ((bytesRead = sceKernelRead(sflashFd, buffer, 4 * 1024 * 1024)) > 0)
		{
			sceKernelWrite(backupFd, buffer, bytesRead);
		}

		free(buffer);
		sceKernelClose(sflashFd);
		sceKernelClose(backupFd);
		return true;
	}

	return false;
}

void SendStatePacket(SceNetId sock, bool succeeded, const char* fmt, ...)
{
	ResultState packet;
	char buffer[1024];

	// Parse the va list.
	va_list args;
	va_start(args, fmt);
	vsprintf(buffer, fmt, args);
	va_end(args);

	// Set up the packet.
	packet.set_succeeded(succeeded);
	packet.set_errormessage(buffer);

	// Send it out!
	SendProtobufPacket(sock, &packet);
}
