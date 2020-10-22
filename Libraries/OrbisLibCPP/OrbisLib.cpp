#include "stdafx.hpp"
#include "OrbisLib.hpp"

OrbisLib::OrbisLib()
{
	//printf("Initializing Classes...\n");

	//Initialize Classes
	this->Proc = new OrbisProc(this);
	this->Target = new OrbisTarget(this);
	this->Debugger = new OrbisDebugger(this);
	this->API = new OrbisAPI(this);
	this->Settings = new OrbisSettings(this);
	this->TargetManagement = new OrbisTargetManagement(this);

	//Since we need to use this dll in the windows service we need to add a check
	if(!NoInstance)
		this->Service = new OrbisService(this);

	//Set the default OrbisLib Port.
	this->Port = 6900;
}

OrbisLib::~OrbisLib()
{
	printf("OrbisLib Destruction!\n");
	//Cleanup.
	delete this->Proc;
	delete this->Target;
	delete this->Debugger;
	delete this->API;
	delete this->Settings;
	delete this->TargetManagement;

	if (!NoInstance)
		delete this->Service;
}

int OrbisLib::TestCommunications(char* IPAddr)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TEST_COMMS;

	return API->Call(IPAddr, &API_Packet);
}