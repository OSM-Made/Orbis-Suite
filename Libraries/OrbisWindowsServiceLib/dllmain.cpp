#include "stdafx.h"

ServiceClient* Client = 0;
bool ServiceRunning = false;
char CurrentTarget[512] = { };
char CurrentProcess[512] = { };

SocketListener* TargetListener = NULL;
SocketListener* PrintListener = NULL;

enum TRAP_FAULTS
{
	T_RESADFLT	= 0,
	T_PRIVINFLT	= 1,
	T_RESOPFLT	= 2,
	T_BPTFLT	= 3,
	T_SYSCALL	= 5,
	T_ARITHTRAP	= 6,
	T_ASTFLT	= 7,
	T_SEGFLT	= 8,
	T_PROTFLT	= 9,
	T_TRCTRAP	= 10,
	T_PAGEFLT	= 12,
	T_TABLEFLT	= 13,
	T_ALIGNFLT	= 14,
	T_KSPNOTVAL	= 15,
	T_BUSERR	= 16,
	T_KDBTRAP	= 17,

	T_DIVIDE	= 18,
	T_NMI		= 19,
	T_OFLOW		= 20,
	T_BOUND		= 21,
	T_DNA		= 22,
	T_DOUBLEFLT	= 23,
	T_FPOPFLT	= 24,
	T_TSSFLT	= 25,
	T_SEGNPFLT	= 26,
	T_STKFLT	= 27,
	T_RESERVED	= 28,
};

enum TargetCommands
{
	CMD_TRAP,
	CMD_BREAK,
	CMD_CONTINUE,
	CMD_PROC_DIE,
	CMD_PROC_ATTACH,
	CMD_PROC_DETACH
};

struct TargetCommandPacket_s
{
	int CommandIndex;
	int TrapFault;
	//ProcName
	//TODO: Add more data
};

DWORD TargetClientThread(LPVOID lpParameter, SOCKET Client)
{
	TargetCommandPacket_s* TargetCommandPacket = (TargetCommandPacket_s*)malloc(sizeof(TargetCommandPacket_s));

	if (recv(Client, (char*)TargetCommandPacket, sizeof(TargetCommandPacket_s), 0))
	{
		switch (TargetCommandPacket->CommandIndex)
		{
		default:
			printf("Dun goofed\n");
			break;

		case CMD_TRAP:

			break;

		case CMD_BREAK:

			break;

		case CMD_CONTINUE:

			break;

		case CMD_PROC_DIE:

			break;

		case CMD_PROC_ATTACH:

			break;

		case CMD_PROC_DETACH:

			break;
		}
	}

	free(TargetCommandPacket);

	//Exit Thread
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

DWORD PrintClientThread(LPVOID lpParameter, SOCKET Client)
{

	//Exit Thread
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

extern "C" __declspec(dllexport) void StartOrbisService(const char* DefaultCOMPort, unsigned short PrintListenerPort, unsigned short CommandListenerPort = 6902, unsigned short TargetListenerPort = 6901)
{
	ServiceRunning = true;

	//Start New Client Manager
	Client = new ServiceClient(CommandListenerPort);

	//Start up listeners
	TargetListener = new SocketListener(TargetClientThread, 0, TargetListenerPort); //Listens for socket commands from the target PS4 ie. Interupts, Change in proc, or attach / dettach
	PrintListener = new SocketListener(PrintClientThread, 0, PrintListenerPort); //Listens on a user defined port for prints from the console. Default for Orbis Suite is 6903


}

extern "C" __declspec(dllexport) void StopOrbisService()
{
	ServiceRunning = false;

	//Clean up
	if (Client)
		delete Client;
	if (TargetListener)
		delete TargetListener;
	if (PrintListener)
		delete PrintListener;
}

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
    }
    return TRUE;
}

