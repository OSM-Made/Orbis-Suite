#pragma once
#include "Export.hpp"

class OrbisProc;
class OrbisTarget;
class OrbisDebugger;
class OrbisService;
class OrbisAPI;
class OrbisSettings;
class OrbisTargetManagement;

class EXPORT OrbisLib
{
public:
	short Port;

	OrbisProc* Proc;
	OrbisTarget* Target;
	OrbisDebugger* Debugger;
	OrbisService* Service;
	OrbisAPI* API;
	OrbisSettings* Settings;
	OrbisTargetManagement* TargetManagement;

	OrbisLib();
	~OrbisLib();
	int TestCommunications(char* IPAddr = NULL);

private:

};