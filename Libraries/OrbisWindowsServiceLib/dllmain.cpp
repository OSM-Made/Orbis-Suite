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

/*
const char* DBname = "Orbis-User-Data.db";

//Set the orbis appdata dir
sprintf(this->OrbisPath, "%s\\Orbis Suite", getenv("APPDATA"));

//Thread to watch db file for changes.
CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)FileWatcherThread, this, 3, NULL);

DWORD WINAPI OrbisTarget::FileWatcherThread(LPVOID Params)
{
	OrbisTarget* orbisTarget = (OrbisTarget*)Params;

	HANDLE hfile = CreateFileA(orbisTarget->OrbisPath,
		FILE_LIST_DIRECTORY,
		FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE,
		NULL,
		OPEN_EXISTING,
		FILE_FLAG_BACKUP_SEMANTICS,
		NULL
	);

	while (orbisTarget->Running)
	{
		FILE_NOTIFY_INFORMATION buffer[1024];
		int returnlen;

		if (ReadDirectoryChangesW(hfile, &buffer, sizeof(buffer), true, FILE_NOTIFY_CHANGE_LAST_WRITE, (LPDWORD)&returnlen, NULL, NULL))
		{
			char* filename = new char[buffer->FileNameLength];
			sprintf(filename, "%ws", buffer->FileName);

			if (buffer[0].Action == FILE_ACTION_MODIFIED && strstr(filename, orbisTarget->DBname))
			{
				orbisTarget->UpdateSettings();

				//TODO: Call a call back ???
			}

			free(filename);
		}
		Sleep(10);
	}

	CloseHandle(hfile);

	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}*/

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

