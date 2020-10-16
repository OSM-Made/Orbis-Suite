#include "stdafx.hpp"
#include "OrbisSettings.hpp"

OrbisSettings::OrbisSettings(OrbisLib* orbisLib)
{
	//Save the orbis lib pointer for use later.
	this->orbisLib = orbisLib;

	//Get the file path for the DB
	char AppdataBuffer[0x100];
	size_t requiredSize = sizeof(AppdataBuffer);
	getenv_s(&requiredSize, (char*)&AppdataBuffer, requiredSize, "PROGRAMDATA");

	//Set the file perms on the folder.
	sprintf_s(this->DBPath, "%s\\Orbis Suite\\", AppdataBuffer);
	SetFilePerms(this->DBPath);

	//Set the DB Path and perms on the db
	sprintf_s(this->DBPath, "%s\\Orbis Suite\\OrbisSuiteUserData", AppdataBuffer);
	SetFilePerms(this->DBPath);

	printf(DBPath);

	//Populate data on startup.
	UpdateSettings();
}

OrbisSettings::~OrbisSettings()
{

}

bool OrbisSettings::OpenDatabase(sqlite3** db)
{
	char* ErrorMsg = 0;
	int rc = 0;

	rc = sqlite3_open(this->DBPath, db);
	if (rc != SQLITE_OK)
		return false;

	sqlite3_busy_timeout(*db, 1000);

	return true;
}

void OrbisSettings::UpdateSettings()
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;

	//Update the settings.
	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT * FROM Settings", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return;
	}

	rc = sqlite3_step(stmt);
	if (rc == SQLITE_ROW)
	{
		this->AutoLoadPayload = sqlite3_column_int(stmt, 0);
		this->StartOnBoot = sqlite3_column_int(stmt, 1);
		this->DetectGame = sqlite3_column_int(stmt, 2);
		strcpy_s(this->COMPort, (const char*)sqlite3_column_text(stmt, 3));
		this->ServicePort = sqlite3_column_int(stmt, 4);
		this->APIPort = sqlite3_column_int(stmt, 5);
		this->CensorIDPS = sqlite3_column_int(stmt, 6);
		this->CensorPSID = sqlite3_column_int(stmt, 7);
		this->Debug = sqlite3_column_int(stmt, 8);
		this->CreateLogs = sqlite3_column_int(stmt, 9);
		this->ShowTimestamps = sqlite3_column_int(stmt, 10);
		this->WordWrap = sqlite3_column_int(stmt, 11);
	}

	sqlite3_finalize(stmt);
	sqlite3_close(db);
}

bool OrbisSettings::SetSettingbyName(const char* Name, int Value)
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

	rc = sqlite3_bind_int(stmt, 1, (int)Value);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Value(int/bool): %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
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

bool OrbisSettings::SetSettingbyName(const char* Name, const char* Value)
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

	rc = sqlite3_bind_text(stmt, 1, (const char*)Value, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Value(text): %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
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