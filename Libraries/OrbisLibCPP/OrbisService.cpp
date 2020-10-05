#include "stdafx.hpp"
#include "OrbisService.hpp"

enum ClientCommands
{
	CMD_CLIENT_CONNECT,
	CMD_CLIENT_DISCONNECT,
	CMD_CLIENT_HEARTBEAT,
	CMD_CLIENT_CHANGE_PRINT_PORT,
	CMD_CLIENT_CHANGE_COM_PORT
};

struct CommandPacket_s
{
	int CommandIndex;
	union
	{
		int Index;
		short PrintPort;
		char COMPort[0x20];
	};
};

OrbisService::OrbisService(OrbisLib* orbisLib)
{
	this->orbisLib = orbisLib;

	//Set Bool to let threads stop when we do clean up.
	this->IsRunning = true;

	//Connect to Windows Service.
	Connect();
}

OrbisService::~OrbisService()
{
	//Signal Threads to stop.
	this->IsRunning = false;

	//Disconnect from windows Service.
	Disconnect();
}

VOID OrbisService::ServiceCallback(LPVOID lpParameter, SOCKET Socket)
{
	OrbisService* orbisService = (OrbisService*)lpParameter;

	//Allocate space to recieve the packet from the Target Console
	TargetCommandPacket_s* Packet = (TargetCommandPacket_s*)malloc(sizeof(TargetCommandPacket_s));

	//Recieve the Targets command packet.
	if (!recv(Socket, (char*)Packet, sizeof(TargetCommandPacket_s), 0))
	{
		printf("Failed to recv TargetCommandPacket\n");

		return;
	}

	switch (Packet->CommandIndex)
	{
	case CMD_PRINT:
		orbisService->HandlePrint(Packet, Socket);
		break;


	case CMD_INTERCEPT:
		printf("TODO: Implement.\n");
		break;

	case CMD_CONTINUE:
		printf("TODO: Implement.\n");
		break;


	case CMD_PROC_DIE:
		printf("TODO: Implement.\n");
		break;

	case CMD_PROC_ATTACH:
		printf("TODO: Implement.\n");
		break;

	case CMD_PROC_DETACH:
		printf("TODO: Implement.\n");
		break;


	case CMD_TARGET_SUSPEND:
		printf("TODO: Implement.\n");
		break;

	case CMD_TARGET_RESUME:
		printf("TODO: Implement.\n");
		break;

	case CMD_TARGET_SHUTDOWN:
		printf("TODO: Implement.\n");
		break;


	case CMD_DB_TOUCHED:
		printf("heh DB was touched.\n");
		orbisService->orbisLib->Target->UpdateSettings();
		break;
	}

	//Clean up.
	free(Packet);
}

bool OrbisService::Connect()
{
	//Shouldnt happen but for safety we have this to make sure we dont connect twice.
	if (this->IsConnectedtoService)
	{
		printf("ServiceClient already connected!\n");

		return false;
	}

	//Set up socket for local host on the command server port
	Sockets* Socket = new Sockets("127.0.0.1", PORT_COMMANDSERVER);

	//connect to the command server.
	if (!Socket->Connect())
	{
		printf("ServiceClient failed to connect. OrbisService failed to connect to the windows service. Is the windows service running?\n");

		return false;
	}

	//Set up the packet for our command.
	CommandPacket_s CommandPacket;
	CommandPacket.CommandIndex = CMD_CLIENT_CONNECT;

	if (!Socket->Send((char*)&CommandPacket, sizeof(CommandPacket_s))) 
	{
		printf("ServiceClient failed to connect. Failed to send packet data.\n");

		free(Socket);

		return false;
	}

	//Reset the client index value and recieve the data.
	this->ClientIndex = -1;
	if (!Socket->Receive((char*)&this->ClientIndex, sizeof(int)))
	{
		printf("ServiceClient failed to connect. Failed to recieve client index from windows service.\n");

		return false;
	}

	//Make sure we have a client index.
	if (this->ClientIndex <= -1 || this->ClientIndex > MAX_CLIENTS)
	{
		printf("ServiceClient failed to connect. %d is not a valid Client Index.\n", this->ClientIndex);

		free(Socket);

		return false;
	}

	printf("ServiceClient Connected with index %d\n", this->ClientIndex);

	//Set our status as connected.
	this->IsConnectedtoService = true;

	//Start up heart beat thread.
	CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)HeartBeatThread, this, 3, NULL);

	//Start up listener.
	ServiceListener = new SocketListener(this->ServiceCallback, this, PORT_START + this->ClientIndex);

	free(Socket);

	return true;
}

DWORD WINAPI OrbisService::HeartBeatThread(LPVOID Params)
{
	OrbisService* orbisService = (OrbisService*)Params;

	while (orbisService->IsRunning && orbisService->IsConnectedtoService)
	{
		orbisService->SendHeartBeat();
		Sleep(8000);
	}
	
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

void OrbisService::Disconnect()
{
	//Make sure we are actually connected before trying to disconnect.
	if (!this->IsConnectedtoService)
	{
		printf("ServiceClient not connected!\n");

		return;
	}

	//Set up socket for local host on the command server port
	Sockets* Socket = new Sockets("127.0.0.1", PORT_COMMANDSERVER);

	//connect to the command server.
	if (!Socket->Connect())
	{
		printf("ServiceClient failed to Disconnect. OrbisService failed to connect to the windows service. Is the windows service running?\n");

		return;
	}

	//Set up the packet for our command.
	CommandPacket_s CommandPacket;
	CommandPacket.CommandIndex = CMD_CLIENT_DISCONNECT;
	CommandPacket.Index = this->ClientIndex;

	//Send the packet.
	if (!Socket->Send((char*)&CommandPacket, sizeof(CommandPacket_s)))
	{
		printf("ServiceClient failed to Disconnect. Failed to send packet data.\n");

		free(Socket);

		return;
	}

	//Reset our connection status.
	this->IsConnectedtoService = false;

	//Destroy Listener
	delete this->ServiceListener;

	free(Socket);
}

void OrbisService::SendHeartBeat()
{
	//Make sure we are actually connected before trying to disconnect.
	if (!this->IsConnectedtoService)
	{
		printf("ServiceClient not connected!\n");

		return;
	}

	//Set up socket for local host on the command server port
	Sockets* Socket = new Sockets("127.0.0.1", PORT_COMMANDSERVER);

	//connect to the command server.
	if (!Socket->Connect())
	{
		printf("ServiceClient failed to SendHeartBeat. OrbisService failed to connect to the windows service. Is the windows service running?\n");

		return;
	}

	//Set up the packet for our command.
	CommandPacket_s CommandPacket;
	CommandPacket.CommandIndex = CMD_CLIENT_HEARTBEAT;
	CommandPacket.Index = this->ClientIndex;

	//Send the packet.
	if (!Socket->Send((char*)&CommandPacket, sizeof(CommandPacket_s)))
	{
		printf("ServiceClient failed to SendHeartBeat. Failed to send packet data.\n");

		free(Socket);

		return;
	}

	free(Socket);
}

void OrbisService::HandlePrint(TargetCommandPacket_s* Packet, SOCKET Socket)
{
	//Alloacate space to recieve print data
	char* Data = (char*)malloc(Packet->Print.Len);

	//Recieve print data
	if (!recv(Socket, Data, Packet->Print.Len, 0))
	{
		printf("Failed to recv Print Data\n");

		return;
	}

	//Do Callbacks for printing data.
	if (Packet->Print.Type == PT_SOCK)
	{
		//TODO: Implement.
	}
	else
	{
		//TODO: Implement.
	}

	free(Data);
}