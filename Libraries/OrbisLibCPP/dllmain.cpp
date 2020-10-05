#include "stdafx.hpp"

OrbisLib* orbisLib;

extern "C" __declspec(dllexport) void dummy()
{
	printf("Hi\n");
}

BOOL APIENTRY DllMain( HMODULE hModule, DWORD  ul_reason_for_call, LPVOID lpReserved )
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
		printf("Test\n");
		orbisLib = new OrbisLib();
		break;

    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

