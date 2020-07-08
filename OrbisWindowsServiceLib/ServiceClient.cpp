#include "stdafx.h"
#include "ServiceClient.h"
#include "Sockets.h"

ServiceClient::ServiceClient() {
	printf("Initialize ServiceClient...\n");

	for (int i = 0; i < MAX_CLIENTS; i++)
	{
		this->ClientInfo[i].Used = false;
		this->ClientInfo[i].Port = PORT_START + i;
		this->ClientInfo[i].LastUpdateTime = 0;
	}

	//Start Thread to monitor Pings
}

ServiceClient::~ServiceClient() {
	printf("Destroy ServiceClient...\n");
}

int ServiceClient::AddClient() {
	//Initialize varible with max clients
	int NewClient = MAX_CLIENTS;

	//loop until we either have reached -1 less than our array bounds or we have found a free client
	do {
		NewClient--;
	} while (NewClient != -1 && this->ClientInfo[NewClient].Used);

	//Make sure we found a free client
	if (NewClient == -1) {
		printf("[Error] No Free Client!\n");
	}
	else {
		//Mark ClientInfo as used
		this->ClientInfo[NewClient].Used = true;
		this->ClientInfo[NewClient].LastUpdateTime = GetTickCount();	
	}
	
	//return our found client or error
	return NewClient;
}

void ServiceClient::RemoveClient(int index) {
	this->ClientInfo[index].Used = false;
}

bool ServiceClient::SendClientPrint(ClientInfo_s ClientInfo, int Type, const char* Data, int length) {
	Sockets* Socket = new Sockets("127.0.0.1", ClientInfo.Port);

	if (!Socket->Connect())
		return false;

	if (!Socket->Send((char*)&Type, sizeof(int))) {
		free(Socket);
		return false;
	}

	if (!Socket->Send((char*)&length, sizeof(int))) {
		free(Socket);
		return false;
	}

	if (!Socket->Send(Data, length)) {
		free(Socket);
		return false;
	}

	free(Socket);
	return true;
}

bool ServiceClient::SendClientInterrupt(ClientInfo_s ClientInfo, ClientInterrupts Interupt) {
	return true;
}

bool ServiceClient::SendProcChange(ClientInfo_s ClientInfo, const char* NewProc) {
	return true;
}

bool ServiceClient::SendProcDetach(ClientInfo_s ClientInfo) {
	return true;
}