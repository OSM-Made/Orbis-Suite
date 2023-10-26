#pragma once
#include "stdafx.h"

class Apps
{
public:
	static void GetDB(SceNetId Sock);
	static void CheckVersion(SceNetId Sock);
	static void GetAppsList(SceNetId Sock);
	static void GetAppInfoString(SceNetId Sock);
	static int GetAppId(const char* TitleId);
	static void SendAppStatus(SceNetId Sock);
	static void StartApp(SceNetId Sock);
	static void KillApp(SceNetId Sock);
	static void SuspendApp(SceNetId Sock);
	static void ResumeApp(SceNetId Sock);
	static void DeleteApp(SceNetId Sock);
	static void SetVisibility(SceNetId Sock);
	static void GetVisibility(SceNetId Sock);
};