#include "stdafx.h"
#include "Apps.h"
#include "AppDatabase.h"
#include <SystemServiceExt.h>
#include <UserServiceExt.h>

#define APP_DB_PATH "/system_data/priv/mms/app.db"

void Apps::GetDB(SceNetId Sock)
{
	//Open file descriptors 
	auto fd = sceKernelOpen(APP_DB_PATH, SCE_KERNEL_O_RDONLY, 0);
	if (fd <= 0)
	{
		Logger::Error("Failed to open app database file.\n");
		return;
	}

	//Get File size
	SceKernelStat stats;
	sceKernelFstat(fd, &stats);

	if (stats.st_size == 0)
	{
		Logger::Error("Failed to get size of app database.\n");
		return;
	}

	//Allocate space to read data.
	auto FileData = (unsigned char*)malloc(stats.st_size);

	//ReadFile.
	sceKernelRead(fd, FileData, stats.st_size);
	sceKernelClose(fd);

	Sockets::SendInt(Sock, stats.st_size);
	Sockets::SendLargeData(Sock, FileData, stats.st_size);

	free(FileData);
}

void Apps::CheckVersion(SceNetId Sock)
{
	auto currentVersion = 0;
	if (!Sockets::RecvInt(Sock, &currentVersion))
	{
		Logger::Error("CheckVersion: Failed to recieve the current app version.\n");
		return;
	}

	Sockets::SendInt(Sock, AppDatabase::GetVersion() > currentVersion ? 1 : 0);
}

// Depreciated ??
void Apps::GetAppsList(SceNetId Sock)
{
	std::vector<AppDatabase::AppInfo> AppList;
	if (!AppDatabase::GetApps(AppList))
	{
		Sockets::SendInt(Sock, 0);
		return;
	}

	Sockets::SendInt(Sock, AppList.size());

	Sockets::SendLargeData(Sock, (unsigned char*)AppList.data(), AppList.size() * sizeof(AppInfoPacket));
}

// Depreciated ??
void Apps::GetAppInfoString(SceNetId Sock)
{
	char titleId[10];
	memset(titleId, 0, sizeof(titleId));
	sceNetRecv(Sock, titleId, sizeof(titleId), 0);

	// Get the key we are interested in.
	char KeyValue[50];
	sceNetRecv(Sock, KeyValue, sizeof(KeyValue), 0);

	// Look up the key for that titleId in the app.db.
	char OutStr[200];
	memset(OutStr, 0, sizeof(OutStr));
	AppDatabase::GetAppInfoString(titleId, OutStr, sizeof(OutStr), KeyValue);

	// Send back the result.
	sceNetSend(Sock, OutStr, sizeof(OutStr), 0);
}

// TODO: Currently cant get the appId of child processes like the Web Browser since it is a child of the ShellUI.
int Apps::GetAppId(const char* TitleId)
{
	int appId = 0;

	// Get the list of running processes.
	std::vector<kinfo_proc> processList;
	GetProcessList(processList);

	for (const auto& i : processList)
	{
		// Get the app info using the pid.
		SceAppInfo appInfo;
		sceKernelGetAppInfo(i.pid, &appInfo);

		// Using the titleId match our desired app and return the appId from the appinfo.
		if (!strcmp(appInfo.TitleId, TitleId))
		{
			appId = appInfo.AppId;

			break;
		}
	}

	return appId;
}

void Apps::SendAppStatus(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Send the happy packet so we continue.
	SendStatePacket(sock, true, "");

	// Get the appId from the titleId.
	auto appId = GetAppId(packet.titleid().c_str());

	// If we have no appId that means the process is not running. 
	if (appId <= 0)
	{
		Sockets::SendInt(sock, STATE_NOT_RUNNING);
	}
	else
	{
		int state = 0;
		auto res = sceSystemServiceIsAppSuspended(appId, &state);
		if (res == 0 && state)
		{
			Sockets::SendInt(sock, STATE_SUSPENDED);
		}
		else
		{
			Sockets::SendInt(sock, STATE_RUNNING);
		}
	}
}

void Apps::StartApp(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	LaunchAppParam appParam;
	appParam.enableCrashReport = 0;
	appParam.checkFlag = 0;
	appParam.appAttr = 0;
	appParam.size = sizeof(LaunchAppParam);

	if (auto res = sceUserServiceGetForegroundUser(&appParam.userId) != 0)
	{
		Logger::Error("sceUserServiceGetForegroundUser(): Failed with error %llX\n", res);
		SendStatePacket(sock, false, "sceUserServiceGetForegroundUser(): Failed with error %llX.", res);
		return;
	}

	auto res = sceLncUtilLaunchApp(packet.titleid().c_str(), nullptr, &appParam);
	if (res <= 0)
	{
		Logger::Error("sceLncUtilLaunchApp() : Failed with error %llX\n", res);
		SendStatePacket(sock, false, "sceLncUtilLaunchApp(): Failed with error %llX.", res);
		return;
	}

	// Send the happy packet so we continue.
	SendStatePacket(sock, true, "");
}

void Apps::KillApp(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Get the appId from the titleId.
	auto appId = GetAppId(packet.titleid().c_str());
	if (appId <= 0)
	{
		SendStatePacket(sock, false, "GetAppId(): Failed to retrieve the app Id.");
		return;
	}

	// Send the command to kill the app by the app Id.
	auto res = sceSystemServiceKillApp(appId, -1, 0, false);
	if (res != 0)
	{
		SendStatePacket(sock, false, "sceSystemServiceKillApp(): Failed with error %llX.", res);
		return;
	}

	// Send the happy packet.
	SendStatePacket(sock, true, "");
}

void Apps::SuspendApp(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Get the appId from the titleId.
	auto appId = GetAppId(packet.titleid().c_str());
	if (appId <= 0)
	{
		SendStatePacket(sock, false, "GetAppId(): Failed to retrieve the app Id.");
		return;
	}

	// Send the lnc command to resume the app using the app id.
	auto res = sceLncUtilSuspendApp(appId, 0);
	if (res != 0)
	{
		SendStatePacket(sock, false, "sceLncUtilSuspendApp(): Failed with error %llX.", res);
		return;
	}

	// Send the happy packet.
	SendStatePacket(sock, true, "");
}

void Apps::ResumeApp(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Get the appId from the titleId.
	auto appId = GetAppId(packet.titleid().c_str());
	if (appId <= 0)
	{
		SendStatePacket(sock, false, "GetAppId(): Failed to retrieve the app Id.");
		return;
	}

	// Call the lnc command to resume the app by the id.
	auto res = sceLncUtilResumeApp(appId, 0);
	if (res != 0)
	{
		SendStatePacket(sock, false, "sceLncUtilResumeApp(): Failed with error %llX.", res);
		return;
	}

	// Set the app as the focused app.
	res = sceApplicationSetApplicationFocus(appId);
	if (res != 0)
	{
		SendStatePacket(sock, false, "sceApplicationSetApplicationFocus(): Failed with error %llX.", res);
		return;
	}

	// Send the happy packet.
	SendStatePacket(sock, true, "");
}

void Apps::DeleteApp(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Call the app util to delete the app. This may only delete the game and patch?
	auto res = sceAppInstUtilAppUnInstall(packet.titleid().c_str());
	if (res != 0)
	{
		SendStatePacket(sock, false, "sceAppInstUtilAppUnInstall(): Failed with error %llX.", res);
		return;
	}

	// Send the happy packet.
	SendStatePacket(sock, true, "");
}

void Apps::SetVisibility(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Send the happy packet so we continue.
	SendStatePacket(sock, true, "");

	auto value = 0;
	if (!Sockets::RecvInt(sock, &value))
	{
		Logger::Error("SetVisibility(): Failed to recieve value.\n");
		SendStatePacket(sock, false, "Failed to recieve value.");
		return;
	}

	if (value >= AppDatabase::VisibilityType::VT_NONE && value <= AppDatabase::VisibilityType::VT_INVISIBLE)
	{
		if (!AppDatabase::SetVisibility(packet.titleid().c_str(), (AppDatabase::VisibilityType)value))
		{
			SendStatePacket(sock, false, "Failed ot set the visibility.");
			return;
		}

		// TODO:
		//ShellUIIPC::RefreshContentArea();

		// Send the happy packet.
		SendStatePacket(sock, true, "");
	}

	SendStatePacket(sock, false, "Failed ot set the visibility.");
}

void Apps::GetVisibility(SceNetId sock)
{
	AppPacket packet;
	if (!RecieveProtoBuf(sock, &packet))
	{
		SendStatePacket(sock, false, "Failed to parse the next protobuf packet.");
		return;
	}

	// Send the happy packet so we continue.
	SendStatePacket(sock, true, "");

	auto visibility = AppDatabase::GetVisibility(packet.titleid().c_str());
	Sockets::SendInt(sock, visibility);
}
