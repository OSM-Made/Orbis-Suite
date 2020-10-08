#pragma once
#include "Export.hpp"

class EXPORT OrbisLib;

class OrbisDebugger
{
private:
	OrbisLib* orbisLib;

public:
	OrbisDebugger(OrbisLib* OrbisLib);
	~OrbisDebugger();
};