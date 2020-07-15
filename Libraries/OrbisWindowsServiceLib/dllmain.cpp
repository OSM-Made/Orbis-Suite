#include "stdafx.h"

ServiceClient* Client = 0;
bool ServiceRunning = false;
char CurrentTarget[512] = { };
char CurrentProcess[512] = { };

SocketListener* TargetListener = NULL;
SocketListener* PrintListener = NULL;

DWORD TargetClientThread(LPVOID lpParameter, SOCKET Client)
{

}

DWORD PrintClientThread(LPVOID lpParameter, SOCKET Client)
{

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

