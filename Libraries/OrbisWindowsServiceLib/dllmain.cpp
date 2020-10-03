#include "stdafx.h"

#define PORT_COMMANDSERVER 6902
#define PORT_TARGETSERVER 6901

ServiceClient* Client = NULL;
SocketListener* TargetListener = NULL;
bool ServiceRunning = false;

const char* TargetCommandsStr[] =
{
	"CMD_PRINT",

	"CMD_INTERCEPT",
	"CMD_CONTINUE",

	"CMD_PROC_DIE",
	"CMD_PROC_ATTACH",
	"CMD_PROC_DETACH",

	"CMD_TARGET_SUSPEND",
	"CMD_TARGET_RESUME",
	"CMD_TARGET_SHUTDOWN"
};

DWORD TargetClientThread(LPVOID lpParameter, SOCKET Socket)
{
	//Allocate space to recieve the packet from the Target Console
	TargetCommandPacket_s* TargetCommandPacket = (TargetCommandPacket_s*)malloc(sizeof(TargetCommandPacket_s));

	//Recieve the Targets command packet.
	recv(Socket, (char*)TargetCommandPacket, sizeof(TargetCommandPacket_s), 0);

	printf("Command Recieved: %d(%s)\n", TargetCommandPacket->CommandIndex, TargetCommandsStr[TargetCommandPacket->CommandIndex]);

	//Forward the packet to all the connected children processes.
	Client->ForwardPacket(TargetCommandPacket);
	
	//Clean up.
	free(TargetCommandPacket);

	//Exit Thread.
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

extern "C" __declspec(dllexport) void dummy()
{

}

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved )
{
    switch (ul_reason_for_call)
    {
		
    case DLL_PROCESS_ATTACH:
		ServiceRunning = true;

		//Start New Client Manager
		Client = new ServiceClient(PORT_COMMANDSERVER);

		//Start up listeners
		TargetListener = new SocketListener(TargetClientThread, 0, PORT_TARGETSERVER); //Listens for socket commands from the target PS4 ie. Interupts, Change in proc, or attach / dettach

		break;

	case DLL_PROCESS_DETACH:
		ServiceRunning = false;

		//Clean up
		if (Client)
			delete Client;
		if (TargetListener)
			delete TargetListener;

		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	
    }
    return TRUE;
}

