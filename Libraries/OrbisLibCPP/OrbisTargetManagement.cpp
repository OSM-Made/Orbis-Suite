#include "stdafx.hpp"
#include "OrbisTargetManagement.hpp"

OrbisTargetManagement::OrbisTargetManagement(OrbisLib* orbisLib)
{
	//Save the orbis lib pointer for use later.s
	this->orbisLib = orbisLib;

	//Get the file path for the DB
	size_t requiredSize = sizeof(ProgramDataBuffer);
	getenv_s(&requiredSize, (char*)&ProgramDataBuffer, requiredSize, "PROGRAMDATA");

	//Set the file perms on the folder.
	sprintf_s(this->DBPath, "%s\\Orbis Suite\\", ProgramDataBuffer);
	SetFilePerms(this->DBPath);

	//Set the DB Path and perms on the db
	sprintf_s(this->DBPath, "%s\\Orbis Suite\\OrbisSuiteUserData", ProgramDataBuffer);
	SetFilePerms(this->DBPath);

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

bool OrbisTargetManagement::DoesDefaultTargetExist()
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
	rc = sqlite3_prepare_v2(db, "SELECT COUNT(*) FROM Targets WHERE DefaultTarget=1", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

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
		Out->PayloadPort = sqlite3_column_int(stmt, 4);
		Out->Available = sqlite3_column_int(stmt, 5);
		strcpy_s(Out->SDKVersion, (const char*)sqlite3_column_text(stmt, 6));
		strcpy_s(Out->SoftwareVersion, (const char*)sqlite3_column_text(stmt, 7));
		strcpy_s(Out->FactorySoftwareVersion, (const char*)sqlite3_column_text(stmt, 8));
		Out->CPUTemp = sqlite3_column_int(stmt, 9);
		Out->SOCTemp = sqlite3_column_int(stmt, 10);
		strcpy_s(Out->CurrentTitleID, (const char*)sqlite3_column_text(stmt, 11));
		strcpy_s(Out->ConsoleName, (const char*)sqlite3_column_text(stmt, 12));
		strcpy_s(Out->MotherboardSerial, (const char*)sqlite3_column_text(stmt, 13));
		strcpy_s(Out->Serial, (const char*)sqlite3_column_text(stmt, 14));
		strcpy_s(Out->Model, (const char*)sqlite3_column_text(stmt, 15));
		strcpy_s(Out->MACAdressLAN, (const char*)sqlite3_column_text(stmt, 16));
		strcpy_s(Out->IDPS, (const char*)sqlite3_column_text(stmt, 17));
		strcpy_s(Out->PSID, (const char*)sqlite3_column_text(stmt, 18));
		strcpy_s(Out->ConsoleType, (const char*)sqlite3_column_text(stmt, 19));
		Out->Attached = sqlite3_column_int(stmt, 20);
		strcpy_s(Out->CurrentProc, (const char*)sqlite3_column_text(stmt, 21));

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
	rc = sqlite3_prepare_v2(db, "UPDATE Targets set DefaultTarget=?, TargetName=?, IPAddress=?, Firmware=?, PayloadPort=? WHERE TargetName=?", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 1, In.Default);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Default: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 2, In.Name, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind TargetName: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 3, In.IPAddr, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind IPAddress: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 4, In.Firmware);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Firmware: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 5, In.PayloadPort);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind PayloadPort: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 6, TargetName, -1, SQLITE_TRANSIENT);
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
		sprintf_s(Targets[ID].FactorySoftwareVersion, "%01X.%03X.%03X", (TargetInfo.FactorySoftwareVersion >> 24) & 0xFF, (TargetInfo.FactorySoftwareVersion >> 12) & 0xFFF, TargetInfo.FactorySoftwareVersion & 0xFFF);
		Targets[ID].CPUTemp = TargetInfo.CPUTemp;
		Targets[ID].SOCTemp = TargetInfo.SOCTemp;
		strcpy_s(Targets[ID].CurrentTitleID, TargetInfo.CurrentTitleID);
		strcpy_s(Targets[ID].ConsoleName, TargetInfo.ConsoleName);
		//strcpy_s(Targets[ID].MotherboardSerial, TargetInfo.MotherboardSerial);
		/*strcpy_s(Targets[ID].Serial, TargetInfo.Serial);
		strcpy_s(Targets[ID].Model, TargetInfo.Model);
		sprintf_s(Targets[ID].MACAdressLAN, "%02X:%02X:%02X:%02X:%02X:%02X",
			TargetInfo.MACAdressLAN[0], TargetInfo.MACAdressLAN[1], TargetInfo.MACAdressLAN[2], 
			TargetInfo.MACAdressLAN[3], TargetInfo.MACAdressLAN[4], TargetInfo.MACAdressLAN[5]);*/

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
	rc = sqlite3_prepare_v2(db, "UPDATE Targets set Available=?, SDKVersion=?, SoftwareVersion=?, FactorySoftwareVersion=?, CPUTemp=?, SOCTemp=?, CurrentTitleID=?, ConsoleName=?, MotherboardSerial=?, Serial=?, Model=?, MACAddressLAN=?, IDPS=?, PSID=?, ConsoleType=?, Attached=?, CurrentProc=? WHERE TargetName=?", -1, &stmt, NULL);
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

	rc = sqlite3_bind_text(stmt, 4, Targets[ID].FactorySoftwareVersion, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind FactorySoftwareVersion: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 5, Targets[ID].CPUTemp);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind CPUTemp: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 6, Targets[ID].SOCTemp);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind SOCTemp: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 7, Targets[ID].CurrentTitleID, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind CurrentTitleID: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 8, Targets[ID].ConsoleName, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind ConsoleName: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 9, Targets[ID].MotherboardSerial, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind MotherboardSerial: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 10, Targets[ID].Serial, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Serial: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 11, Targets[ID].Model, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Model: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 12, Targets[ID].MACAdressLAN, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind MACAdressLAN: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 13, Targets[ID].IDPS, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind IDPS: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 14, Targets[ID].PSID, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind PSID: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 15, Targets[ID].ConsoleType, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind ConsoleType: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_int(stmt, 16, Targets[ID].Attached);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind Attached: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 17, Targets[ID].CurrentProc, -1, SQLITE_TRANSIENT);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind CurrentProc: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return false;
	}

	rc = sqlite3_bind_text(stmt, 18, Targets[ID].Name, -1, SQLITE_TRANSIENT);
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

	//Makes sure there is always a default target.
	if (!DoesDefaultTargetExist())
		In.Default = true;

	if (!OpenDatabase(&db))
	{
		printf("Failed to open database: %s\n", sqlite3_errmsg(db));

		return false;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "INSERT INTO Targets (TargetName, IPAddress, Firmware, PayloadPort) VALUES (?, ?, ?, ?)", -1, &stmt, NULL);
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

	rc = sqlite3_bind_int(stmt, 4, In.PayloadPort);
	if (rc != SQLITE_OK)
	{
		printf("Failed to bind PayloadPort: %s\n", sqlite3_errmsg(db));

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
		Out->PayloadPort = sqlite3_column_int(stmt, 4);
		Out->Available = sqlite3_column_int(stmt, 5);
		strcpy_s(Out->SDKVersion, (const char*)sqlite3_column_text(stmt, 6));
		strcpy_s(Out->SoftwareVersion, (const char*)sqlite3_column_text(stmt, 7));
		strcpy_s(Out->FactorySoftwareVersion, (const char*)sqlite3_column_text(stmt, 8));
		Out->CPUTemp = sqlite3_column_int(stmt, 9);
		Out->SOCTemp = sqlite3_column_int(stmt, 10);
		strcpy_s(Out->CurrentTitleID, (const char*)sqlite3_column_text(stmt, 11));
		strcpy_s(Out->ConsoleName, (const char*)sqlite3_column_text(stmt, 12));
		strcpy_s(Out->MotherboardSerial, (const char*)sqlite3_column_text(stmt, 13));
		strcpy_s(Out->Serial, (const char*)sqlite3_column_text(stmt, 14));
		strcpy_s(Out->Model, (const char*)sqlite3_column_text(stmt, 15));
		strcpy_s(Out->MACAdressLAN, (const char*)sqlite3_column_text(stmt, 16));
		strcpy_s(Out->IDPS, (const char*)sqlite3_column_text(stmt, 17));
		strcpy_s(Out->PSID, (const char*)sqlite3_column_text(stmt, 18));
		strcpy_s(Out->ConsoleType, (const char*)sqlite3_column_text(stmt, 19));
		Out->Attached = sqlite3_column_int(stmt, 20);
		strcpy_s(Out->CurrentProc, (const char*)sqlite3_column_text(stmt, 21));

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

		return 0;
	}

	sqlite3_stmt *stmt;
	rc = sqlite3_prepare_v2(db, "SELECT COUNT(*) FROM Targets", -1, &stmt, NULL);
	if (rc != SQLITE_OK)
	{
		printf("Failed to prep stmt: %s\n", sqlite3_errmsg(db));

		sqlite3_close(db);
		return 0;
	}

	rc = sqlite3_step(stmt);
	if (rc != SQLITE_ERROR)
		Count = sqlite3_column_int(stmt, 0);

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
		Out[LoopCount].PayloadPort = sqlite3_column_int(stmt, 4);
		Out[LoopCount].Available = sqlite3_column_int(stmt, 5);
		strcpy_s(Out[LoopCount].SDKVersion, (const char*)sqlite3_column_text(stmt, 6));
		strcpy_s(Out[LoopCount].SoftwareVersion, (const char*)sqlite3_column_text(stmt, 7));
		strcpy_s(Out[LoopCount].FactorySoftwareVersion, (const char*)sqlite3_column_text(stmt, 8));
		Out[LoopCount].CPUTemp = sqlite3_column_int(stmt, 9);
		Out[LoopCount].SOCTemp = sqlite3_column_int(stmt, 10);
		strcpy_s(Out[LoopCount].CurrentTitleID, (const char*)sqlite3_column_text(stmt, 11));
		strcpy_s(Out[LoopCount].ConsoleName, (const char*)sqlite3_column_text(stmt, 12));
		strcpy_s(Out[LoopCount].MotherboardSerial, (const char*)sqlite3_column_text(stmt, 13));
		strcpy_s(Out[LoopCount].Serial, (const char*)sqlite3_column_text(stmt, 14));
		strcpy_s(Out[LoopCount].Model, (const char*)sqlite3_column_text(stmt, 15));
		strcpy_s(Out[LoopCount].MACAdressLAN, (const char*)sqlite3_column_text(stmt, 16));
		strcpy_s(Out[LoopCount].IDPS, (const char*)sqlite3_column_text(stmt, 17));
		strcpy_s(Out[LoopCount].PSID, (const char*)sqlite3_column_text(stmt, 18));
		strcpy_s(Out[LoopCount].ConsoleType, (const char*)sqlite3_column_text(stmt, 19));
		Out[LoopCount].Attached = sqlite3_column_int(stmt, 20);
		strcpy_s(Out[LoopCount].CurrentProc, (const char*)sqlite3_column_text(stmt, 21));

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