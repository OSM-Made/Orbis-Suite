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
	"CMD_TARGET_SHUTDOWN",

	"CMD_DB_TOUCHED"
};

VOID TargetClientThread(LPVOID lpParameter, SOCKET Socket)
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
}

const char* DBname = "Orbis-User-Data.db";
char OrbisPath[0x1000];

DWORD WINAPI FileWatcherThread(LPVOID Params)
{
	HANDLE hfile = CreateFileA(OrbisPath,
		FILE_LIST_DIRECTORY,
		FILE_SHARE_READ | FILE_SHARE_WRITE | FILE_SHARE_DELETE,
		NULL,
		OPEN_EXISTING,
		FILE_FLAG_BACKUP_SEMANTICS,
		NULL
	);

	while (ServiceRunning)
	{
		FILE_NOTIFY_INFORMATION buffer[1024];
		int returnlen;

		if (ReadDirectoryChangesW(hfile, &buffer, sizeof(buffer), true, FILE_NOTIFY_CHANGE_LAST_WRITE, (LPDWORD)&returnlen, NULL, NULL))
		{
			char* filename = new char[buffer->FileNameLength];
			sprintf_s(filename, buffer->FileNameLength, "%ws", buffer->FileName);

			if (buffer[0].Action == FILE_ACTION_MODIFIED && strstr(filename, DBname))
			{
				//Set up packet to send.
				TargetCommandPacket_s TargetCommandPacket;
				TargetCommandPacket.CommandIndex = CMD_DB_TOUCHED;

				//Forward the packet to all the connected children processes.
				Client->ForwardPacket(&TargetCommandPacket);

				printf("Sent Command %d(CMD_DB_TOUCHED)\n", CMD_DB_TOUCHED);
			}

			free(filename);
		}
		Sleep(10);
	}

	CloseHandle(hfile);

	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

extern "C" __declspec(dllexport) void dummy()
{

}

extern "C" _declspec(dllimport) void SetupCPP(bool WinService);
extern "C" __declspec(dllimport) void DestroyCPP();

void OrbisStartService()
{
	//Setup our OrbisLib instance for WinService.
	SetupCPP(true);

	ServiceRunning = true;

	//Start New Client Manager
	Client = new ServiceClient(PORT_COMMANDSERVER);

	//Start up listeners
	TargetListener = new SocketListener(TargetClientThread, 0, PORT_TARGETSERVER); //Listens for socket commands from the target PS4 ie. Interupts, Change in proc, or attach / dettach

	//Set the orbis appdata dir
	char AppdataBuffer[0x100];
	size_t requiredSize = sizeof(AppdataBuffer);
	getenv_s(&requiredSize, (char*)&AppdataBuffer, requiredSize, "APPDATA");

	sprintf_s(OrbisPath, sizeof(OrbisPath), "%s\\Orbis Suite", AppdataBuffer);

	//Thread to watch db file for changes.
	CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)FileWatcherThread, NULL, 3, NULL);
}

void OrbisEndService()
{
	ServiceRunning = false;

	//Clean up
	if (Client)
		delete Client;

	if (TargetListener)
		delete TargetListener;

	//Clean up our OrbisLib instance.
	DestroyCPP();
}

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved )
{
    switch (ul_reason_for_call)
    {
		
    case DLL_PROCESS_ATTACH:
		OrbisStartService();

		break;

	case DLL_PROCESS_DETACH:
		OrbisEndService();

		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	
    }
    return TRUE;
}

