#pragma once
class OrbisLib;

#include "Export.hpp"
#include "sqlite3.h"

class EXPORT OrbisTargetManagement
{
private:
	OrbisLib* orbisLib;

	const char* DBname = "Orbis-User-Data.db";
	char DBPath[0x200];

	bool OpenDatabase(sqlite3** db);
	bool GetDefaultTarget(DB_TargetInfo* Out);

public:
	OrbisTargetManagement(OrbisLib* orbisLib);
	~OrbisTargetManagement();

	char ProgramDataBuffer[0x100];

	DB_TargetInfo Targets[MAX_TARGETS];
	int TargetCount;

	//Target Management
	bool DoesDefaultTargetExist();
	bool DoesTargetExist(const char* TargetName);
	bool DoesTargetExistIP(const char* IPAddr);
	bool GetTarget(const char* TargetName, DB_TargetInfo* Out);
	bool SetTarget(const char* TargetName, DB_TargetInfo In);
	bool UpdateTargetExtInfo(int Target);
	bool DeleteTarget(const char* TargetName);
	bool NewTarget(DB_TargetInfo In);
	bool SetDefaultTarget(const char* Name);
	void UpdateTargets();

	//Target List
	int GetTargetCount();
	bool GetTargetList(DB_TargetInfo Out[]);
};