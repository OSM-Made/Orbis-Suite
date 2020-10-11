#include "stdafx.hpp"
#include "OrbisTargetManagement.hpp"

OrbisTargetManagement::OrbisTargetManagement(OrbisLib* orbisLib)
{
	//Save the orbis lib pointer for use later.s
	this->orbisLib = orbisLib;

	//Get the file path for the DB
	char AppdataBuffer[0x100];
	size_t requiredSize = sizeof(AppdataBuffer);
	getenv_s(&requiredSize, (char*)&AppdataBuffer, requiredSize, "APPDATA");
	sprintf_s(this->DBPath, "%s\\Orbis Suite\\Orbis-User-Data.db", AppdataBuffer);

	//Make sure DB Exists if not write default db
	if (!FileExists(this->DBPath))
	{
		printf("DB doesnt Exist, Creating default DB.\n");
		//CWriteFile(this->DBPath, &this->DefaultDB, sizeof(this->DefaultDB));
	}

	//Populate structures on startup.
	UpdateTargets();
}

OrbisTargetManagement::~OrbisTargetManagement()
{

}

bool OrbisTargetManagement::OpenDatabase(sqlite3** db)
{
	char* ErrorMsg = 0;
	int rc = 0;

	rc = sqlite3_open(this->DBPath, db);
	if (rc != SQLITE_OK)
		return false;

	sqlite3_busy_timeout(*db, 1000);

	return true;
}

bool OrbisTargetManagement::DoesTargetExist(const char* TargetName)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;
	bool Result = false;

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT COUNT(*) FROM Targets WHERE TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, TargetName, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind text: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_step(stmt);
	if (rc != SQLITE_ERROR)
	{
		if (sqlite3_column_int(stmt, 0) > 0)
			Result = true;
		else
			Result = false;
	}
	else
	{
		printf("Failed to read row: %s\n", sqlite3_errmsg(db));

		Result = false;
	}

	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return Result;
}

bool OrbisTargetManagement::DoesTargetExistIP(const char* IPAddr)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;
	bool Result = false;

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT COUNT(*) FROM Targets WHERE IPaddress=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, IPAddr, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind text: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_step(stmt);
	if (rc != SQLITE_ERROR)
	{
		if (sqlite3_column_int(stmt, 0) > 0)
			Result = true;
		else
			Result = false;
	}
	else
	{
		printf("Failed to read row: %s\n", sqlite3_errmsg(db));

		Result = false;
	}

	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return Result;
}

bool OrbisTargetManagement::GetTarget(const char* TargetName, DB_TargetInfo* Out)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;
	bool Result = false;

	if (!DoesTargetExist(TargetName))
	{
		printf("Target \"%s\" doesn't exist in Targets Table.\n", TargetName);

		return false;
	}

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT * FROM Targets WHERE TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, TargetName, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind text: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}


	rc = sqlite3_step(stmt);
	if (rc == SQLITE_ROW)
	{
		Out->Default = sqlite3_column_int(stmt, 0);
		strcpy_s(Out->Name, (const char*)sqlite3_column_text(stmt, 1));
		strcpy_s(Out->IPAddr, (const char*)sqlite3_column_text(stmt, 2));
		Out->Firmware = sqlite3_column_int(stmt, 3);
		Out->Available = sqlite3_column_int(stmt, 4);
		strcpy_s(Out->SDKVersion, (const char*)sqlite3_column_text(stmt, 5));
		strcpy_s(Out->SoftwareVersion, (const char*)sqlite3_column_text(stmt, 6));
		Out->CPUTemp = sqlite3_column_int(stmt, 7);
		Out->SOCTemp = sqlite3_column_int(stmt, 8);
		strcpy_s(Out->CurrentTitleID, (const char*)sqlite3_column_text(stmt, 9));
		strcpy_s(Out->ConsoleName, (const char*)sqlite3_column_text(stmt, 10));
		strcpy_s(Out->IDPS, (const char*)sqlite3_column_text(stmt, 11));
		strcpy_s(Out->PSID, (const char*)sqlite3_column_text(stmt, 12));
		strcpy_s(Out->ConsoleType, (const char*)sqlite3_column_text(stmt, 13));
		Out->Attached = sqlite3_column_int(stmt, 14);
		strcpy_s(Out->CurrentProc, (const char*)sqlite3_column_text(stmt, 15));

		Result = true;
	}
	else
	{
		printf("Failed to read row: %s\n", sqlite3_errmsg(db));

		Result = false;
	}

	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return Result;
}

bool OrbisTargetManagement::SetTarget(const char* TargetName, DB_TargetInfo In)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;

	if (!DoesTargetExist(TargetName))
	{
		printf("Target \"%s\" doesn't exist in Targets Table.\n", TargetName);

		return false;
	}

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "UPDATE Targets set TargetName=?, IPAddress=?, Firmware=? WHERE TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, In.Name, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind TargetName: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 2, In.IPAddr, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind IPAddress: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 3, In.Firmware);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Firmware: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 4, TargetName, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind OldTargetName: %s\n", sqlite3_errmsg(db));

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

	if (In.Default)
		this->SetDefaultTarget(In.Name);

	UpdateTargets();

	return true;
}

bool OrbisTargetManagement::UpdateTargetExtInfo(int ID)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;

	UpdateTargets();

	if (!DoesTargetExist(Targets[ID].Name))
	{
		printf("Target \"%s\" doesn't exist in Targets Table.\n", Targets[ID].Name);

		return false;
	}

	RESP_TargetInfo TargetInfo;
	if (orbisLib->Target->GetInfo(Targets[ID].IPAddr, &TargetInfo) != API_OK)
	{
		Targets[ID].Available = false;
		strcpy_s(Targets[ID].CurrentTitleID, "-");
		Targets[ID].CPUTemp = 0;
		Targets[ID].SOCTemp = 0;
		Targets[ID].Attached = 0;
		strcpy_s(Targets[ID].CurrentProc, "-");
	}
	else
	{
		Targets[ID].Available = true;
		sprintf_s(Targets[ID].SDKVersion, "%01X.%03X.%03X", (TargetInfo.SDKVersion >> 24) & 0xFF, (TargetInfo.SDKVersion >> 12) & 0xFFF, TargetInfo.SDKVersion & 0xFFF);
		sprintf_s(Targets[ID].SoftwareVersion, "%01X.%02X", (TargetInfo.SoftwareVersion >> 24) & 0xFF, (TargetInfo.SoftwareVersion >> 16) & 0xFF);
		Targets[ID].CPUTemp = TargetInfo.CPUTemp;
		Targets[ID].SOCTemp = TargetInfo.SOCTemp;
		strcpy_s(Targets[ID].CurrentTitleID, TargetInfo.CurrentTitleID);
		strcpy_s(Targets[ID].ConsoleName, TargetInfo.ConsoleName);
		sprintf_s(Targets[ID].IDPS, "%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X",
			(TargetInfo.IDPS[0] & 0xffU),
			(TargetInfo.IDPS[1] & 0xffU),
			(TargetInfo.IDPS[2] & 0xffU),
			(TargetInfo.IDPS[3] & 0xffU),
			(TargetInfo.IDPS[4] & 0xffU),
			(TargetInfo.IDPS[5] & 0xffU),
			(TargetInfo.IDPS[6] & 0xffU),
			(TargetInfo.IDPS[7] & 0xffU),
			(TargetInfo.IDPS[8] & 0xffU),
			(TargetInfo.IDPS[9] & 0xffU),
			(TargetInfo.IDPS[10] & 0xffU),
			(TargetInfo.IDPS[11] & 0xffU),
			(TargetInfo.IDPS[12] & 0xffU),
			(TargetInfo.IDPS[13] & 0xffU),
			(TargetInfo.IDPS[14] & 0xffU),
			(TargetInfo.IDPS[15] & 0xffU));
		sprintf_s(Targets[ID].PSID, "%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X%02X",
			(TargetInfo.PSID[0] & 0xffU),
			(TargetInfo.PSID[1] & 0xffU),
			(TargetInfo.PSID[2] & 0xffU),
			(TargetInfo.PSID[3] & 0xffU),
			(TargetInfo.PSID[4] & 0xffU),
			(TargetInfo.PSID[5] & 0xffU),
			(TargetInfo.PSID[6] & 0xffU),
			(TargetInfo.PSID[7] & 0xffU),
			(TargetInfo.PSID[8] & 0xffU),
			(TargetInfo.PSID[9] & 0xffU),
			(TargetInfo.PSID[10] & 0xffU),
			(TargetInfo.PSID[11] & 0xffU),
			(TargetInfo.PSID[12] & 0xffU),
			(TargetInfo.PSID[13] & 0xffU),
			(TargetInfo.PSID[14] & 0xffU),
			(TargetInfo.PSID[15] & 0xffU));
		strcpy_s(Targets[ID].ConsoleType, ConsoleTypeNames[TargetInfo.ConsoleType]);
		Targets[ID].Attached = TargetInfo.Attached;
		strcpy_s(Targets[ID].CurrentProc, TargetInfo.CurrentProc);
	}

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "UPDATE Targets set Available=?, SDKVersion=?, SoftwareVersion=?, CPUTemp=?, SOCTemp=?, CurrentTitleID=?, ConsoleName=?, IDPS=?, PSID=?, ConsoleType=? WHERE TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 1, Targets[ID].Available);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Available: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 2, Targets[ID].SDKVersion, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind SDKVersion: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 3, Targets[ID].SoftwareVersion, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind SoftwareVersion: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 4, Targets[ID].CPUTemp);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind CPUTemp: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 5, Targets[ID].SOCTemp);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind SOCTemp: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 6, Targets[ID].CurrentTitleID, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind CurrentTitleID: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 7, Targets[ID].ConsoleName, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind ConsoleName: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 8, Targets[ID].IDPS, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind IDPS: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 9, Targets[ID].PSID, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind PSID: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 10, Targets[ID].ConsoleType, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind ConsoleType: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 11, Targets[ID].Name, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Name: %s\n", sqlite3_errmsg(db));

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

	return true;
}

bool OrbisTargetManagement::DeleteTarget(const char* TargetName)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;

	if (!DoesTargetExist(TargetName))
	{
		printf("Target \"%s\" doesn't exist in Targets Table.\n", TargetName);

		return false;
	}

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "DELETE FROM Targets where TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, TargetName, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind text: %s\n", sqlite3_errmsg(db));

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

	UpdateTargets();

	return true;
}

bool OrbisTargetManagement::NewTarget(DB_TargetInfo In)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;

	if (DoesTargetExist(In.Name))
	{
		printf("Target \"%s\" already Exists!!\n", In.Name);

		return false;
	}

	if (GetTargetCount() >= MAX_TARGETS)
	{
		printf("Maximum number of stored targets reached %d.", MAX_TARGETS);

		return false;
	}

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "INSERT INTO Targets (TargetName, IPAddress, Firmware) VALUES (?, ?, ?)", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, In.Name, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind TargetName: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 2, In.IPAddr, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind IPAddress: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 3, In.Firmware);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Firmware: %s\n", sqlite3_errmsg(db));

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

	if (In.Default)
		this->SetDefaultTarget(In.Name);

	UpdateTargets();

	return true;
}

bool OrbisTargetManagement::SetDefaultTarget(const char* Name)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;

	//Make sure Target exists in list before we set default target that doesnt exist.
	if (!DoesTargetExist(Name))
	{
		printf("Cant set \"%s\" as Default Target as it doesnt exist in Targets Table.\n", Name);

		return false;
	}

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "UPDATE Targets Set DefaultTarget=0 WHERE DefaultTarget=1", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

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

	rc = sqlite3_prepare_v2(db, "UPDATE Targets SET DefaultTarget=1 WHERE TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 1, Name, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind text: %s\n", sqlite3_errmsg(db));

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

	UpdateTargets();

	return true;
}

bool OrbisTargetManagement::GetDefaultTarget(DB_TargetInfo* Out)
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;
	bool Result = false;

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT * FROM Targets where DefaultTarget=1", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_step(stmt);
	if (rc == SQLITE_ROW)
	{
		Out->Default = sqlite3_column_int(stmt, 0);
		strcpy_s(Out->Name, (const char*)sqlite3_column_text(stmt, 1));
		strcpy_s(Out->IPAddr, (const char*)sqlite3_column_text(stmt, 2));
		Out->Firmware = sqlite3_column_int(stmt, 3);
		Out->Available = sqlite3_column_int(stmt, 4);
		strcpy_s(Out->SDKVersion, (const char*)sqlite3_column_text(stmt, 5));
		strcpy_s(Out->SoftwareVersion, (const char*)sqlite3_column_text(stmt, 6));
		Out->CPUTemp = sqlite3_column_int(stmt, 7);
		Out->SOCTemp = sqlite3_column_int(stmt, 8);
		strcpy_s(Out->CurrentTitleID, (const char*)sqlite3_column_text(stmt, 9));
		strcpy_s(Out->ConsoleName, (const char*)sqlite3_column_text(stmt, 10));
		strcpy_s(Out->IDPS, (const char*)sqlite3_column_text(stmt, 11));
		strcpy_s(Out->PSID, (const char*)sqlite3_column_text(stmt, 12));
		strcpy_s(Out->ConsoleType, (const char*)sqlite3_column_text(stmt, 13));
		Out->Attached = sqlite3_column_int(stmt, 14);
		strcpy_s(Out->CurrentProc, (const char*)sqlite3_column_text(stmt, 15));

		Result = true;
	}
	else
	{
		printf("Failed to read row: %s\n", sqlite3_errmsg(db));

		Result = false;
	}

	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return Result;
}

int OrbisTargetManagement::GetTargetCount()
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;
	int Count = 0;

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT COUNT(*) FROM Targets", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_step(stmt);
	if (rc != SQLITE_ERROR)
		Count = sqlite3_column_int(stmt, 0);
	else
		printf("Failed to read row: %s\n", sqlite3_errmsg(db));

	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return Count;
}

bool OrbisTargetManagement::GetTargetList(DB_TargetInfo Out[])
{
	sqlite3* db;
	char* ErrorMsg = 0;
	int rc = 0;
	int LoopCount = 0;

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT * FROM Targets", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	while (sqlite3_step(stmt) == SQLITE_ROW)
	{
		if (LoopCount > MAX_TARGETS)
			break;

		Out[LoopCount].Default = sqlite3_column_int(stmt, 0);
		strcpy_s(Out[LoopCount].Name, (const char*)sqlite3_column_text(stmt, 1));
		strcpy_s(Out[LoopCount].IPAddr, (const char*)sqlite3_column_text(stmt, 2));
		Out[LoopCount].Firmware = sqlite3_column_int(stmt, 3);
		Out[LoopCount].Available = sqlite3_column_int(stmt, 4);
		strcpy_s(Out[LoopCount].SDKVersion, (const char*)sqlite3_column_text(stmt, 5));
		strcpy_s(Out[LoopCount].SoftwareVersion, (const char*)sqlite3_column_text(stmt, 6));
		Out[LoopCount].CPUTemp = sqlite3_column_int(stmt, 7);
		Out[LoopCount].SOCTemp = sqlite3_column_int(stmt, 8);
		strcpy_s(Out[LoopCount].CurrentTitleID, (const char*)sqlite3_column_text(stmt, 9));
		strcpy_s(Out[LoopCount].ConsoleName, (const char*)sqlite3_column_text(stmt, 10));
		strcpy_s(Out[LoopCount].IDPS, (const char*)sqlite3_column_text(stmt, 11));
		strcpy_s(Out[LoopCount].PSID, (const char*)sqlite3_column_text(stmt, 12));
		strcpy_s(Out[LoopCount].ConsoleType, (const char*)sqlite3_column_text(stmt, 13));
		Out[LoopCount].Attached = sqlite3_column_int(stmt, 14);
		strcpy_s(Out[LoopCount].CurrentProc, (const char*)sqlite3_column_text(stmt, 15));

		LoopCount++;
	}

	sqlite3_finalize(stmt);
	sqlite3_close(db);

	return true;
}

void OrbisTargetManagement::UpdateTargets()
{
	//Update the TargetList.
	GetTargetList(this->Targets);
	this->TargetCount = GetTargetCount();

	//Get the target struct for default Target.
	GetDefaultTarget(&this->orbisLib->Settings->DefaultTarget);
}