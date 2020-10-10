#include "stdafx.hpp"
#include "OrbisAPI.hpp"

OrbisAPI::OrbisAPI(OrbisLib* orbisLib)
{
	this->orbisLib = orbisLib;
}

OrbisAPI::~OrbisAPI()
{

}

int OrbisAPI::Connect(Sockets** Sockin, char* IPAddr)
{
	Sockets* Sock;
	//Get the default Target IP or custom set IP if neither we return fail.
	if (IPAddr == NULL || !strcmp(IPAddr, ""))
	{
		if (orbisLib->Target->Settings.DefaultTarget.IPAddr == NULL || !strcmp(orbisLib->Target->Settings.DefaultTarget.IPAddr, ""))
			return API_ERROR_NOTARGET;

		Sock = new Sockets(orbisLib->Target->Settings.DefaultTarget.IPAddr, this->Port);
	}
	else
		Sock = new Sockets(IPAddr, this->Port);

	if (!Sock->Connect())
		return API_ERROR_FAILED_TO_CONNNECT;

	*Sockin = Sock;

	return API_OK;
}

int OrbisAPI::Call(char* IPAddr, API_Packet_s* API_Packet)
{
	Sockets* Sock;
	int Status = Connect(&Sock, IPAddr);

	if (Status != API_OK)
		return Status;

	if (!Sock->Send((char*)API_Packet, sizeof(API_Packet_s))) {
		Sock->Close();
		return API_ERROR_FAIL;
	}

	if (!Sock->Receive((char*)&Status, sizeof(int))) {
		Sock->Close();
		return API_ERROR_FAIL;
	}

	Sock->Close();
	delete Sock;
	return Status;
}

int OrbisAPI::CallLong(Sockets** Sockin, char* IPAddr, API_Packet_s* API_Packet)
{
	int Status = Connect(Sockin, IPAddr);

	if (Status != API_OK)
		return Status;

	Sockets* Sock = *Sockin;

	if (!Sock->Send((char*)API_Packet, sizeof(API_Packet_s))) {
		Sock->Close();
		return API_ERROR_FAIL;
	}

	if (!Sock->Receive((char*)&Status, sizeof(int))) {
		Sock->Close();
		return API_ERROR_FAIL;
	}

	return Status;
}

void OrbisAPI::FinishCall(Sockets** Sockin)
{
	Sockets* Sock = *Sockin;
	Sock->Close();
	delete Sock;
}