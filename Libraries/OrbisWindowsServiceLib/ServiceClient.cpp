#include "stdafx.h"
#include "ServiceClient.h"
#include "Sockets.h"

VOID ServiceClient::CommandClientThread(LPVOID lpParameter, SOCKET Client) {
	int Index = -1;

	ServiceClient* serviceClient = (ServiceClient*)lpParameter;
	CommandPacket_s* CommandPacket = (CommandPacket_s*)malloc(sizeof(CommandPacket_s));
	if (recv(Client, (char*)CommandPacket, sizeof(CommandPacket_s), 0))
	{
		switch (CommandPacket->CommandIndex) //TODO: Add Command For changing Print Socket Port and COM Port
		{
		default:
			socketprint("[Error] Client Command Thread Recieve Invalid Command Index!\n");
			break;

		case CMD_CLIENT_CONNECT:
			Index = serviceClient->AddClient();

			send(Client, (char*)&Index, sizeof(int), 0);

			break;

		case CMD_CLIENT_DISCONNECT:

			serviceClient->RemoveClient(CommandPacket->Index);

			break;

		case CMD_CLIENT_HEARTBEAT:
			if ((serviceClient->ClientInfo[CommandPacket->Index].Used == false) || (GetTickCount() - serviceClient->ClientInfo[CommandPacket->Index].LastUpdateTime) > 10000) 
			{
				socketprint("Client has timed out Sending reconnect request.\n");
				int Status = 1;
				send(Client, (char*)&Status, sizeof(int), 0);
			}
			else
			{
				socketprint("Client heart beat Packet took %dms to respond\n", (GetTickCount() - serviceClient->ClientInfo[CommandPacket->Index].LastUpdateTime));

				serviceClient->ClientInfo[CommandPacket->Index].LastUpdateTime = GetTickCount();

				int Status = 2;
				send(Client, (char*)&Status, sizeof(int), 0);
			}
			break;
		}
	}
}

DWORD ServiceClient::SocketAliveCheck(LPVOID ptr)
{
	ServiceClient* serviceClient = (ServiceClient*)ptr;
	while (serviceClient->ServiceRunning)
	{
		for (int i = 0; i < MAX_CLIENTS; i++)
		{
			if (serviceClient->ClientInfo[i].Used && ((GetTickCount() - serviceClient->ClientInfo[i].LastUpdateTime) > 10000))
			{
				socketprint("No response from Client %i in > 10000ms. Timed out!\n", i);

				serviceClient->ClientInfo[i].Used = false;
				serviceClient->ClientInfo[i].LastUpdateTime = 0;
			}
		}
		Sleep(10);
	}

	//Exit Thread
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

ServiceClient::ServiceClient(unsigned short CommandListenerPort) {
	socketprint("ServiceClient Initialization...\n");

	//Initialize Struct
	for (int i = 0; i < MAX_CLIENTS; i++)
	{
		this->ClientInfo[i].Used = false;
		this->ClientInfo[i].Port = PORT_START + i;
		this->ClientInfo[i].LastUpdateTime = 0;
	}

	this->ServiceRunning = true;

	//Start Thread to monitor Pings
	DWORD hThreadID;
	HANDLE hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)SocketAliveCheck, this, 3, &hThreadID);
	ResumeThread(hThread);
	CloseHandle(hThread);

	//Start Listener to Commands from clients
	this->CommandListener = new SocketListener(&ServiceClient::CommandClientThread, this, CommandListenerPort);
}

ServiceClient::~ServiceClient() {
	socketprint("ServiceClient Destruction...\n");

	//Notify Thread to close
	this->ServiceRunning = false;

	//Clean Up
	if (this->CommandListener)
		free(this->CommandListener);
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
		socketprint("[Error] No Free Client!\n");
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

void ServiceClient::ForwardPacket(TargetCommandPacket_s* TargetCommandPacket)
{
	for (int i = 0; i < MAX_CLIENTS; i++)
	{
		ClientInfo_s* ClientInfo = &this->ClientInfo[i];
		if (ClientInfo->Used)
		{
			socketprint("Sending Packet to 127.0.0.1:%d...\n", ClientInfo->Port);

			Sockets* Socket = new Sockets("127.0.0.1", ClientInfo->Port);

			if (!Socket->Connect()) {
				socketprint("Failed to Connect on 127.0.0.1:%d\n", ClientInfo->Port);
				continue;
			}

			if (!Socket->Send((char*)TargetCommandPacket, sizeof(TargetCommandPacket_s))) {
				socketprint("Failed to Send Command Packet.\n");
				free(Socket);
				continue;
			}

			free(Socket);
		}
	}
}