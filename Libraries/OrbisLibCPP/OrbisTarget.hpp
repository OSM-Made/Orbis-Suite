#pragma once
class OrbisLib;

#include "Export.hpp"
#include "sqlite3.h"

#define MAX_TARGETS 20

class EXPORT OrbisTarget
{
private:
	OrbisLib* orbisLib;

public:
	OrbisTarget(OrbisLib* OrbisLib);
	~OrbisTarget();

	//API Calls
	int GetInfo(char* IPAddr, RESP_TargetInfo* TargetInfo);
	int Shutdown(char* IPAddr);
	int Reboot(char* IPAddr);
	int Suspend(char* IPAddr);
	int Notify(char* IPAddr, int Type, const char* Message);
	int Beep(char* IPAddr, int Count);
	int SetLED(char* IPAddr, char R, char G, char B, char A);
	int GetLED(char* IPAddr, char* R, char* G, char* B, char* A);
	int DumpProcess(char* IPAddr, const char* ProcName, uint64_t* Size, char* Out);

};
