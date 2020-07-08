#include "stdafx.h"
#include "ServiceLib.h"

#pragma region Command Client

DWORD ServiceLib::CommandClientThread(LPVOID lpParameter, SOCKET Client) {

	ServiceLib* serviceLib = (ServiceLib*)lpParameter;
	CommandPacket_s* CommandPacket = (CommandPacket_s*)malloc(sizeof(CommandPacket_s));
	if (recv(Client, (char*)CommandPacket, sizeof(CommandPacket_s), 0))
	{
		switch (CommandPacket->CommandNumber)
		{
		default:
			printf("[Error] Client Command Thread Recieve Invalid Command Index!\n");
			break;

		case CMD_GET_CURRENT_TARGET:

			break;
		case CMD_SET_CURRENT_TARGET:
			strcpy_s(serviceLib->CurrentTarget, 512, CommandPacket->SetCurrentTargetPacket.TargetName);
			break;
		}
	}

	return 0;
}

#pragma endregion

ServiceLib::ServiceLib(unsigned short CommandListenerPort, unsigned short TargetListenerPort) {
	this->ServiceRunning = true;

	//Initialize Client List class
	this->Client = new ServiceClient();

	//Start Thread to recieve Commands from console

	//Start Thread to recieve SocketPrints

	

	//Start Listener to Commands from clients
	this->CommandListener = new SocketListener(&ServiceLib::CommandClientThread, this, CommandListenerPort);
}

ServiceLib::~ServiceLib() {
	ServiceRunning = false;

	//Clean Up
	if (this->Client)
		free(this->Client);

	if (this->CommandListener)
		free(this->CommandListener);

	if (this->TargetListener)
		free(this->TargetListener);

	if (this->LogListener)
		free(this->LogListener);
}