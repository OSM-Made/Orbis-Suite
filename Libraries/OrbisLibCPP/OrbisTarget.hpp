#pragma once
class OrbisLib;

#include "Export.hpp"
#include "sqlite3.h"

#define MAX_TARGETS 20

class EXPORT OrbisTarget
{
private:
	OrbisLib* orbisLib;

	static unsigned char DefaultDB[12288];
	const char* DBname = "Orbis-User-Data.db";
	char DBPath[0x200];
	bool Running;

	bool OpenDatabase(sqlite3** db);
	bool GetDefaultTarget(DB_TargetInfo* Out);

public:
	struct 
	{
		bool AutoLoadPayload;
		bool StartOnBoot;
		bool DetectGame;
		DB_TargetInfo DefaultTarget;
		char COMPort[0x50];
		short ServicePort;
		short APIPort;
		bool CensorIDPS;
		bool CensorPSID;
		bool Debug;
		bool CreateLogs;
		bool ShowTimestamps;
		bool WordWrap;
	}Settings;
	


	DB_TargetInfo Targets[MAX_TARGETS];
	int TargetCount;

	OrbisTarget(OrbisLib* OrbisLib);
	~OrbisTarget();

	//Target Management
	bool DoesTargetExist(const char* TargetName);
	bool DoesTargetExistIP(const char* IPAddr);
	bool GetTarget(const char* TargetName, DB_TargetInfo* Out);
	bool SetTarget(const char* TargetName, DB_TargetInfo In);
	bool UpdateTargetExtInfo(int Target);
	bool DeleteTarget(const char* TargetName);
	bool NewTarget(DB_TargetInfo In);
	bool SetDefaultTarget(const char* Name);

	//Target List
	int GetTargetCount();
	bool GetTargetList(DB_TargetInfo Out[]);

	//Settings
	void UpdateSettings();
	template<typename p1>
	bool SetSettingbyName(const char* Name, p1 Value)
	{
		sqlite3* db;
		char* ErrorMsg = 0;
		int rc = 0;

		if (!OpenDatabase(&db))
		{
			printf("Failed to open database: %s\n", sqlite3_errmsg(db));

			return false;
		}

		char stmtString[0x200];
		sprintf_s(stmtString, "UPDATE Settings SET %s=?", Name);

		sqlite3_stmt *stmt;
		rc = sqlite3_prepare_v2(db, stmtString, -1, &stmt, NULL);
		if (rc != SQLITE_OK)
		{
			printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

			sqlite3_close(db);
			return false;
		}

		if ((std::is_same<p1, int>::value) || (std::is_same<p1, bool>::value))
		{
			rc = sqlite3_bind_int(stmt, 1, (int)Value);
			if (rc != SQLITE_OK)
			{
				printf("Failed to bind Value(int/bool): %s\n", sqlite3_errmsg(db));

				sqlite3_close(db);
				return false;
			}
		}
		else
		{
			rc = sqlite3_bind_text(stmt, 1, (const char*)Value, -1, SQLITE_TRANSIENT);
			if (rc != SQLITE_OK)
			{
				printf("Failed to bind Value(text): %s\n", sqlite3_errmsg(db));

				sqlite3_close(db);
				return false;
			}
		}

		rc = sqlite3_step(stmt);
		if (rc != SQLITE_DONE)
		{
			printf("Failed to step: %s\n", sqlite3_errmsg(db));

			sqlite3_finalize(stmt);
			sqlite3_close(db);
			return false;
		}

		sqlite3_finalize(stmt);
		sqlite3_close(db);

		UpdateSettings();

		return true;
	}
	bool SetAutoLoadPayload(bool Value);
	bool SetStartOnBoot(bool Value);
	

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
