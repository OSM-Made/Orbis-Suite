#include "stdafx.hpp"

EXPORT OrbisLib* orbisLib;

#pragma region Exports

#pragma region OrbisService

extern "C"  __declspec(dllexport) void OrbisService_RegisterCallBacks(
	Target_Print_Callback Target_Print,
	Proc_Intercept_Callback Proc_Intercept,
	Proc_Continue_Callback Proc_Continue,
	Proc_Die_Callback Proc_Die,
	Proc_Attach_Callback Proc_Attach,
	Proc_Detach_Callback Proc_Detach,
	Target_Suspend_Callback Target_Suspend,
	Target_Resume_Callback Target_Resume,
	Target_Shutdown_Callback Target_Shutdown,
	Target_NewTitle_Callback Target_NewTitle,
	DB_Touched_Callback DB_Touched,
	Target_Availability_Callback Target_Availability)
{
	orbisLib->Service->Target_Print = Target_Print;
	orbisLib->Service->Proc_Intercept = Proc_Intercept;
	orbisLib->Service->Proc_Continue = Proc_Continue;
	orbisLib->Service->Proc_Die = Proc_Die;
	orbisLib->Service->Proc_Attach = Proc_Attach;
	orbisLib->Service->Proc_Detach = Proc_Detach;
	orbisLib->Service->Target_Suspend = Target_Suspend;
	orbisLib->Service->Target_Resume = Target_Resume;
	orbisLib->Service->Target_Shutdown = Target_Shutdown;
	orbisLib->Service->Target_NewTitle = Target_NewTitle;
	orbisLib->Service->DB_Touched = DB_Touched;
	orbisLib->Service->Target_Availability = Target_Availability;
}

#pragma endregion

#pragma region OrbisLib

extern "C" __declspec(dllexport) int TestCommunications(const char* IPAddr)
{
	return orbisLib->TestCommunications((char*)IPAddr);
}

#pragma endregion

#pragma region OrbisTarget

extern "C" __declspec(dllexport) bool DoesTargetExist(const char* TargetName)
{
	return orbisLib->Target->DoesTargetExist(TargetName);
}

extern "C" __declspec(dllexport) bool DoesTargetExistIP(const char* IPAddr)
{
	return orbisLib->Target->DoesTargetExistIP(IPAddr);
}

extern "C" __declspec(dllexport) bool GetTarget(const char* TargetName, DB_TargetInfo* Out)
{
	return orbisLib->Target->GetTarget(TargetName, Out);
}

extern "C" __declspec(dllexport) bool SetTarget(const char* TargetName, bool Default, const char* NewTargetName, const char* IPAddr, int Firmware)
{
	DB_TargetInfo In;
	In.Default = Default;
	strcpy_s(In.Name, NewTargetName);
	strcpy_s(In.IPAddr, IPAddr);
	In.Firmware = Firmware;

	return orbisLib->Target->SetTarget(TargetName, In);
}

extern "C" __declspec(dllexport) bool DeleteTarget(const char* TargetName)
{
	return orbisLib->Target->DeleteTarget(TargetName);
}

extern "C" __declspec(dllexport) bool NewTarget(bool Default, const char* TargetName, const char* IPAddr, int Firmware)
{
	DB_TargetInfo In;
	In.Default = Default;
	strcpy_s(In.Name, TargetName);
	strcpy_s(In.IPAddr, IPAddr);
	In.Firmware = Firmware;

	return orbisLib->Target->NewTarget(In);
}

extern "C" __declspec(dllexport) int GetTargetCount()
{
	return orbisLib->Target->TargetCount;
}

extern "C" __declspec(dllexport) int GetTargets(uint64_t* Targets)
{
	*Targets = (uint64_t)&orbisLib->Target->Targets;
	return orbisLib->Target->TargetCount;
}

extern "C" __declspec(dllexport) bool GetAutoLoadPayload()
{
	return orbisLib->Target->AutoLoadPayload;
}

extern "C" __declspec(dllexport) void SetAutoLoadPayload(bool Value)
{
	orbisLib->Target->SetAutoLoadPayload(Value);
}

extern "C" __declspec(dllexport) bool GetStartOnBoot()
{
	return orbisLib->Target->StartOnBoot;
}

extern "C" __declspec(dllexport) void SetStartOnBoot(bool Value)
{
	orbisLib->Target->SetStartOnBoot(Value);
}

extern "C" __declspec(dllexport) void GetDefaultTarget(DB_TargetInfo* DefaultTarget)
{
	*DefaultTarget = orbisLib->Target->DefaultTarget;
}

extern "C" __declspec(dllexport) void SetDefault(const char* TargetName)
{
	orbisLib->Target->SetDefaultTarget(TargetName);
}

//TODO: Remove
/*extern "C" __declspec(dllexport) int GetInfo(char* IPAddr, RESP_TargetInfo* TargetInfo)
{
	return orbisLib->Target->GetInfo(IPAddr, TargetInfo);
}*/

extern "C" __declspec(dllexport) int Shutdown(char* IPAddr)
{
	return orbisLib->Target->Shutdown(IPAddr);
}

extern "C" __declspec(dllexport) int Reboot(char* IPAddr)
{
	return orbisLib->Target->Reboot(IPAddr);
}

extern "C" __declspec(dllexport) int Suspend(char* IPAddr)
{
	return orbisLib->Target->Suspend(IPAddr);
}

extern "C" __declspec(dllexport) int Notify(char* IPAddr, int Type, const char* Message)
{
	return orbisLib->Target->Notify(IPAddr, Type, Message);
}

extern "C" __declspec(dllexport) int DoBeep(char* IPAddr, int Count)
{
	return orbisLib->Target->Beep(IPAddr, Count);
}

extern "C" __declspec(dllexport) int SetLED(char* IPAddr, char R, char G, char B, char A)
{
	return orbisLib->Target->SetLED(IPAddr, R, G, B, A);
}

extern "C" __declspec(dllexport) int GetLED(char* IPAddr, char* R, char* G, char* B, char* A)
{
	return orbisLib->Target->GetLED(IPAddr, R, G, B, A);
}

extern "C" __declspec(dllexport) int DumpProcess(char* IPAddr, const char* ProcName, uint64_t* Size, char* Out)
{
	return orbisLib->Target->DumpProcess(IPAddr, ProcName, Size, Out);
}

#pragma endregion

#pragma endregion

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

