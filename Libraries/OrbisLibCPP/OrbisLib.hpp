#pragma once
class OrbisProc;
class OrbisTarget;
class OrbisDebugger;
class OrbisService;

class OrbisLib
{
public:
	short Port;

	OrbisProc* Proc;
	OrbisTarget* Target;
	OrbisDebugger* Debugger;
	OrbisService* Service;

	OrbisLib();
	~OrbisLib();
	int TestCommunications();

private:

};