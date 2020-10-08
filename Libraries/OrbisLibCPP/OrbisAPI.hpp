#pragma once
#include "Export.hpp"

class OrbisLib;

class OrbisAPI
{
private:
	OrbisLib* orbisLib;

	int Connect(Sockets** Sockin, char* IPAddr);
	short Port = 6900;

public:
	OrbisAPI(OrbisLib* orbisLib);
	~OrbisAPI();

	int Call(char* IPAddr, API_Packet_s* API_Packet);
	int CallLong(Sockets** Sock, char* IPAddr, API_Packet_s* API_Packet);
	void FinishCall(Sockets** Sockin);
};