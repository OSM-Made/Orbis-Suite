#include "stdafx.h"

VOID TargetClientThread(LPVOID lpParameter, SOCKET Socket)
{
	char* Buffer = NULL;

	//Get the sender IP
	struct sockaddr_in addr;
	int addr_size = sizeof(struct sockaddr_in);
	getpeername(Socket, (struct sockaddr *)&addr, &addr_size);

	//Allocate space to recieve the packet from the Target Console
	TargetCommandPacket_s* TargetCommandPacket = (TargetCommandPacket_s*)malloc(sizeof(TargetCommandPacket_s));

	//Recieve the Targets command packet.
	recv(Socket, (char*)TargetCommandPacket, sizeof(TargetCommandPacket_s), 0);

	//validate command
	if (TargetCommandPacket->CommandIndex < 0 || TargetCommandPacket->CommandIndex > CMD_TARGET_AVAILABILITY)
		goto CleanUp;

	//Write IP to packet.
	strcpy_s(TargetCommandPacket->IPAddr, inet_ntoa(addr.sin_addr));

	if (TargetCommandPacket->CommandIndex == CMD_TARGET_NEWTITLE)
	{
		int TargetCount = orbisLib->TargetManagement->GetTargetCount();
		if (TargetCount > 0)
		{
			for (int i = 0; i < TargetCount; i++)
			{
				orbisLib->TargetManagement->UpdateTargetExtInfo(i);
			}
		}
	}

	printf("Command Recieved: %d(%s)\n", TargetCommandPacket->CommandIndex, TargetCommandsStr[TargetCommandPacket->CommandIndex]);

	if (TargetCommandPacket->CommandIndex == CMD_PRINT) 
	{
		FileIO::FileWrite("C:\\Users\\Gregory\\Source\\Repos\\Orbis Suite\\Executables\\OrbisSuiteService\\bin\\x64\\Debug\\Test.bin", (char*)TargetCommandPacket, sizeof(TargetCommandPacket_s));
		printf("Sender: %s\n", TargetCommandPacket->Print.Sender);
		printf("Data: %s\n", TargetCommandPacket->Print.Data);
	}
	else if(TargetCommandPacket->CommandIndex == CMD_TARGET_NEWTITLE)
		printf("%s\n", TargetCommandPacket->TitleChange.TitleID);
	//printf("%llX\n", TargetCommandPacket);

	//Forward the packet to all the connected children processes.
	Client->ForwardPacket(TargetCommandPacket);
	
CleanUp:
	//Clean up.
	free(TargetCommandPacket);
	free(Buffer);
}

const char* DBname = "OrbisSuiteUserData";
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

				//Update our local DB aswell.
				orbisLib->TargetManagement->UpdateTargets();
				orbisLib->Settings->UpdateSettings();
			}

			free(filename);
		}
		Sleep(10);
	}

	CloseHandle(hfile);

	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

extern "C" __declspec(dllexport) void StartLib()
{
	//program data path
	char ProgramDataPath[MAX_PATH];
	size_t requiredSize = sizeof(ProgramDataPath);
	getenv_s(&requiredSize, (char*)&ProgramDataPath, requiredSize, "PROGRAMDATA");

	//EnableDebugLogs();

	ServiceRunning = true;

	//Start New Client Manager
	Client = new ServiceClient(PORT_COMMANDSERVER);

	//Start up listeners
	TargetListener = new SocketListener(TargetClientThread, 0, PORT_TARGETSERVER); //Listens for socket commands from the target PS4 ie. Interupts, Change in proc, or attach / dettach

	//Set the orbis appdata dir
	sprintf_s(OrbisPath, sizeof(OrbisPath), "%s\\Orbis Suite", ProgramDataPath);

	//Thread to watch db file for changes.
	CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)FileWatcherThread, NULL, 3, NULL);

	//Setup our OrbisLib instance for WinService.
	SetupCPP(true);

	//Thread to check for Target Availability
	TargetWatcher = new ServiceTargetWatcher();
}

extern "C" __declspec(dllexport) void StopLib()
{
	printf("StopLib Called!!!\n");

	ServiceRunning = false;

	//Clean up
	if (Client)
		delete Client;

	if (TargetListener)
		delete TargetListener;

	if (TargetWatcher)
		delete TargetWatcher;

	//Clean up our OrbisLib instance.
	DestroyCPP();
}

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved )
{
    switch (ul_reason_for_call)
    {
		
    case DLL_PROCESS_ATTACH:
		break;

	case DLL_PROCESS_DETACH:
		printf("DLL_PROCESS_DETACH\n");
		break;

	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
		break;
	
    }
    return TRUE;
}

