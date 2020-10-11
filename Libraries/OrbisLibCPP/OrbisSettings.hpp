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

	bool SetSettingbyName(const char* Name, int Value);
	bool SetSettingbyName(const char* Name, const char* Value);
};