#include "stdafx.hpp"
#include "OrbisLib.hpp"

OrbisLib::OrbisLib()
{
	//Initialize Classes
	this->Proc = new OrbisProc(this);
	this->Target = new OrbisTarget(this);
	this->Debugger = new OrbisDebugger(this);
	this->Service = new OrbisService(this);

	//Set the default OrbisLib Port.
	this->Port = 6900;

}

OrbisLib::~OrbisLib()
{

}

int OrbisLib::TestCommunications()
{
	Sockets* Sock = new Sockets(Target->DefaultTarget.IPAddr, this->Port);

	if (!Sock->Connect())
		return false;

	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TEST_COMMS;

	if (!Sock->Send((char*)&API_Packet, sizeof(API_Packet_s))) {
		Sock->Close();
		return false;
	}

	int Status = 0;
	if (!Sock->Receive((char*)&Status, sizeof(int))) {
		Sock->Close();
		return false;
	}

	Sock->Close();
	return Status;
}