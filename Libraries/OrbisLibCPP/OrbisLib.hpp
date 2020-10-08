#pragma once
#include "Export.hpp"

class OrbisProc;
class OrbisTarget;
class OrbisDebugger;
class OrbisService;
class OrbisAPI;

class EXPORT OrbisLib
{
public:
	short Port;

	OrbisProc* Proc;
	OrbisTarget* Target;
	OrbisDebugger* Debugger;
	OrbisService* Service;
	OrbisAPI* API;

	OrbisLib();
	~OrbisLib();
	int TestCommunications(char* IPAddr = NULL);

private:

};