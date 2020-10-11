#pragma once
class OrbisLib;

#include "Export.hpp"
#include "sqlite3.h"


class EXPORT OrbisSettings
{
private:
	OrbisLib* orbisLib;

	const char* DBname = "Orbis-User-Data.db";
	char DBPath[0x200];


public:
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

	OrbisSettings(OrbisLib* orbisLib);
	~OrbisSettings();

	bool OpenDatabase(sqlite3** db);
	void UpdateSettings();

	template<typename p1> bool SetSettingbyName(const char* Name, p1 Value);
};

template<typename p1>
bool OrbisSettings::SetSettingbyName(const char* Name, p1 Value)
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