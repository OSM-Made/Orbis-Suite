#include "stdafx.hpp"
#include "OrbisLib.hpp"

OrbisLib::OrbisLib()
{
	//Initialize Classes
	this->Proc = new OrbisProc(this);
	this->Target = new OrbisTarget(this);
	this->Debugger = new OrbisDebugger(this);
	this->API = new OrbisAPI(this);

	//Since we need to use this dll in the windows service we need to add a check
	if(!IsWinService)
		this->Service = new OrbisService(this);

	//Set the default OrbisLib Port.
	this->Port = 6900;

}

OrbisLib::~OrbisLib()
{
	printf("Destruction!\n");
	//Cleanup.
	delete this->Proc;
	delete this->Target;
	delete this->Debugger;
	if (!IsWinService)
		delete this->Service;
}

int OrbisLib::TestCommunications(char* IPAddr)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TEST_COMMS;

	return API->Call(IPAddr, &API_Packet);
}