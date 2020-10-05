#include "stdafx.hpp"

OrbisLib* orbisLib;

extern "C" __declspec(dllexport) void test()
{
	printf("Hi\n");
}

extern "C"  __declspec(dllexport) void SetupCPP(bool WinService)
{
	IsWinService = WinService;
	orbisLib = new OrbisLib();
}

extern "C" __declspec(dllexport) void DestroyCPP()
{
	delete orbisLib;
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

