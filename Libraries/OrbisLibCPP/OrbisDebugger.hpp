#pragma once
class OrbisLib;

class OrbisDebugger
{
private:
	OrbisLib* orbisLib;

public:
	OrbisDebugger(OrbisLib* OrbisLib);
	~OrbisDebugger();
};