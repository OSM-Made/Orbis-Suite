#pragma once
class OrbisLib;
#include "sqlite3.h"

class OrbisTarget
{
private:
	static unsigned char DefaultDB[12288];
	const char* DBname = "Orbis-User-Data.db";
	char DBPath[0x200];
	bool Running;

	bool OpenDatabase(sqlite3** db);
	bool DoesTargetExist(const char* TargetName);
	bool GetDefaultTarget(char* NameOut);

	//Depreciated
	static DWORD WINAPI FileWatcherThread(LPVOID Params);

public:
	bool AutoConnect;
	bool AutoLoadPayload;
	bool AutoLoadOrbisLib;
	DB_TargetInfo DefaultTarget;

	OrbisTarget(OrbisLib* OrbisLib);
	~OrbisTarget();

	//Target Management
	bool GetTarget(const char* TargetName, DB_TargetInfo* Out);
	bool SetTarget(const char* TargetName, DB_TargetInfo In);
	bool DeleteTarget(const char* TargetName);
	bool NewTarget(DB_TargetInfo In);

	//Target List
	int GetTargetCount();
	bool GetTargetList(DB_TargetInfo Out[]);

	//Settings
	void UpdateSettings();
	bool SetAutoConnect(bool Value);
	bool SetAutoLoadPayload(bool Value);
	bool SetAutoLoadOrbisLib(bool Value);
	bool SetDefaultTarget(const char* Name);
	void GetDefaultTargetInfo(DB_TargetInfo* Out);

};
