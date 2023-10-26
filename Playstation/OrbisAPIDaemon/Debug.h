#pragma once
#include "ProcessMonitor.h"

class Debug
{
public:
	static bool IsDebugging;
	static int CurrentPID;

	static bool CheckDebug(SceNetId s);
	static void Attach(SceNetId sock);
	static void Detach(SceNetId Sock);
	static void Current(SceNetId sock);
	static void RWMemory(SceNetId Sock, bool write);

private:
	static std::mutex DebugMtx;
	static std::shared_ptr<ProcessMonitor> DebuggeeMonitor;

	static bool TryDetach(int pid);

	// Process Events
	static void OnExit();
	static void OnException(int status);
};
