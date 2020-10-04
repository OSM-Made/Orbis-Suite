#pragma once
class OrbisProc;
class OrbisTarget;
class OrbisDebugger;

class OrbisLib
{
public:
	char IPAddress[16];
	short Port;

	OrbisProc* Proc;
	OrbisTarget* Target;
	OrbisDebugger* Debugger;

	OrbisLib();
	~OrbisLib();
	int TestCommunications();

private:

};