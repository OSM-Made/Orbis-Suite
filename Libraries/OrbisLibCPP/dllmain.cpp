#include "stdafx.hpp"

__declspec(dllexport) OrbisLib* orbisLib;

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
	if (orbisLib->Service)
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
}

#pragma endregion

#pragma region OrbisLib

extern "C" __declspec(dllexport) int TestCommunications(char* IPAddr)
{
	return orbisLib->TestCommunications(IPAddr);
}

#pragma endregion

#pragma region OrbisProc

extern "C" __declspec(dllexport) int GetProcList(char* IPAddr, int32_t* ProcCount, char* List)
{
	return orbisLib->Proc->GetList(IPAddr, ProcCount, List);
}

extern "C" __declspec(dllexport) int Attach(char* IPAddr, char* ProcName)
{
	return orbisLib->Proc->Attach(IPAddr, ProcName);
}

extern "C" __declspec(dllexport) int Detach(char* IPAddr, char* ProcName)
{
	return orbisLib->Proc->Detach(IPAddr, ProcName);
}

extern "C" __declspec(dllexport) int GetCurrent(char* IPAddr, RESP_Proc* Out)
{
	return orbisLib->Proc->GetCurrent(IPAddr, Out);
}

extern "C" __declspec(dllexport) int Read(char* IPAddr, uint64_t Address, size_t Len, char* Data)
{
	return orbisLib->Proc->Read(IPAddr, Address, Len, Data);
}

extern "C" __declspec(dllexport) int Write(char* IPAddr, uint64_t Address, size_t Len, char* Data)
{
	return orbisLib->Proc->Write(IPAddr, Address, Len, Data);
}

extern "C" __declspec(dllexport) int Kill(char* IPAddr, char* ProcName)
{
	return orbisLib->Proc->Kill(IPAddr, ProcName);
}

extern "C" __declspec(dllexport) int LoadELF(char* IPAddr, char* Buffer, size_t Len)
{
	return orbisLib->Proc->LoadELF(IPAddr, Buffer, Len);
}

//TODO: Add RPC Call.

/*
Modules
*/
extern "C" __declspec(dllexport) int32_t LoadSPRX(char* IPAddr, char* Path, int32_t Flags)
{
	return orbisLib->Proc->LoadSPRX(IPAddr, Path, Flags);
}

extern "C" __declspec(dllexport) int UnloadSPRX(char* IPAddr, int32_t Handle, int32_t Flags)
{
	return orbisLib->Proc->UnloadSPRX(IPAddr, Handle, Flags);
}

extern "C" __declspec(dllexport) int UnloadSPRXbyName(char* IPAddr, char* Name, int32_t Flags)
{
	return orbisLib->Proc->UnloadSPRX(IPAddr, Name, Flags);
}

extern "C" __declspec(dllexport) int32_t ReloadSPRXbyName(char* IPAddr, char* Name, int32_t Flags)
{
	return orbisLib->Proc->ReloadSPRX(IPAddr, Name, Flags);
}

extern "C" __declspec(dllexport) int32_t ReloadSPRX(char* IPAddr, int32_t Handle, int32_t Flags)
{
	return orbisLib->Proc->ReloadSPRX(IPAddr, Handle, Flags);
}

extern "C" __declspec(dllexport) int DumpModule(char* IPAddr, char* ModuleName, int* Len, char* Out)
{
	return orbisLib->Proc->DumpModule(IPAddr, ModuleName, Len, Out);
}

extern "C" __declspec(dllexport) int GetLibraryList(char* IPAddr, int32_t* ModuleCount, char* Out)
{
	return orbisLib->Proc->GetLibraryList(IPAddr, ModuleCount, Out);
}

#pragma endregion

#pragma region OrbisTarget

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

#pragma region TargetManagement

extern "C" __declspec(dllexport) bool DoesDefaultTargetExist()
{
	return orbisLib->TargetManagement->DoesDefaultTargetExist();
}

extern "C" __declspec(dllexport) bool DoesTargetExist(const char* TargetName)
{
	return orbisLib->TargetManagement->DoesTargetExist(TargetName);
}

extern "C" __declspec(dllexport) bool DoesTargetExistIP(const char* IPAddr)
{
	return orbisLib->TargetManagement->DoesTargetExistIP(IPAddr);
}

extern "C" __declspec(dllexport) bool GetTarget(const char* TargetName, DB_TargetInfo* Out)
{
	return orbisLib->TargetManagement->GetTarget(TargetName, Out);
}

extern "C" __declspec(dllexport) bool SetTarget(const char* TargetName, bool Default, const char* NewTargetName, const char* IPAddr, int Firmware, int PayloadPort)
{
	DB_TargetInfo In;
	In.Default = Default;
	strcpy_s(In.Name, NewTargetName);
	strcpy_s(In.IPAddr, IPAddr);
	In.Firmware = Firmware;
	In.PayloadPort = PayloadPort;

	return orbisLib->TargetManagement->SetTarget(TargetName, In);
}

extern "C" __declspec(dllexport) bool DeleteTarget(const char* TargetName)
{
	return orbisLib->TargetManagement->DeleteTarget(TargetName);
}

extern "C" __declspec(dllexport) bool NewTarget(bool Default, const char* TargetName, const char* IPAddr, int Firmware, int PayloadPort)
{
	DB_TargetInfo In;
	In.Default = Default;
	strcpy_s(In.Name, TargetName);
	strcpy_s(In.IPAddr, IPAddr);
	In.Firmware = Firmware;
	In.PayloadPort = PayloadPort;

	return orbisLib->TargetManagement->NewTarget(In);
}

extern "C" __declspec(dllexport) int GetTargetCount()
{
	return orbisLib->TargetManagement->TargetCount;
}

extern "C" __declspec(dllexport) int GetTargets(uint64_t* Targets)
{
	*Targets = (uint64_t)&orbisLib->TargetManagement->Targets;
	return orbisLib->TargetManagement->TargetCount;
}

extern "C" __declspec(dllexport) void GetDefaultTarget(DB_TargetInfo* DefaultTarget)
{
	*DefaultTarget = orbisLib->Settings->DefaultTarget;
}

extern "C" __declspec(dllexport) void SetDefault(const char* TargetName)
{
	orbisLib->TargetManagement->SetDefaultTarget(TargetName);
}

#pragma endregion

#pragma region Settings

extern "C" __declspec(dllexport) bool GetAutoLoadPayload()
{
	return orbisLib->Settings->AutoLoadPayload;
}

extern "C" __declspec(dllexport) void SetAutoLoadPayload(bool Value)
{
	orbisLib->Settings->SetSettingbyName("AutoLoadPayload", Value);
}

extern "C" __declspec(dllexport) bool GetStartOnBoot()
{
	return orbisLib->Settings->StartOnBoot;
}

extern "C" __declspec(dllexport) void SetStartOnBoot(bool Value)
{
	orbisLib->Settings->SetSettingbyName("StartOnBoot", Value);
}

extern "C" __declspec(dllexport) bool GetDetectGame()
{
	return orbisLib->Settings->DetectGame;
}

extern "C" __declspec(dllexport) void SetDetectGame(bool Value)
{
	orbisLib->Settings->SetSettingbyName("DetectGame", Value);
}

extern "C" __declspec(dllexport) char* GetCOMPort()
{
	return (char*)&orbisLib->Settings->COMPort;
}

extern "C" __declspec(dllexport) void SetCOMPort(char* Value)
{
	orbisLib->Settings->SetSettingbyName("COMPort", Value);
}

extern "C" __declspec(dllexport) int GetServicePort()
{
	return orbisLib->Settings->ServicePort;
}

extern "C" __declspec(dllexport) void SetServicePort(int Value)
{
	orbisLib->Settings->SetSettingbyName("ServicePort", Value);
}

extern "C" __declspec(dllexport) int GetAPIPort()
{
	return orbisLib->Settings->APIPort;
}

extern "C" __declspec(dllexport) void SetAPIPort(int Value)
{
	orbisLib->Settings->SetSettingbyName("APIPort", Value);
}

extern "C" __declspec(dllexport) bool GetCensorIDPS()
{
	return orbisLib->Settings->CensorIDPS;
}

extern "C" __declspec(dllexport) void SetCensorIDPS(bool Value)
{
	orbisLib->Settings->SetSettingbyName("CensorIDPS", Value);
}

extern "C" __declspec(dllexport) bool GetCensorPSID()
{
	return orbisLib->Settings->CensorPSID;
}

extern "C" __declspec(dllexport) void SetCensorPSID(bool Value)
{
	orbisLib->Settings->SetSettingbyName("CensorPSID", Value);
}

extern "C" __declspec(dllexport) bool GetDebug()
{
	return orbisLib->Settings->Debug;
}

extern "C" __declspec(dllexport) void SetDebug(bool Value)
{
	orbisLib->Settings->SetSettingbyName("Debug", Value);
}
extern "C" __declspec(dllexport) bool GetCreateLogs()
{
	return orbisLib->Settings->CreateLogs;
}

extern "C" __declspec(dllexport) void SetCreateLogs(bool Value)
{
	orbisLib->Settings->SetSettingbyName("CreateLogs", Value);
}
extern "C" __declspec(dllexport) bool GetShowTimestamps()
{
	return orbisLib->Settings->ShowTimestamps;
}

extern "C" __declspec(dllexport) void SetShowTimestamps(bool Value)
{
	orbisLib->Settings->SetSettingbyName("ShowTimestamps", Value);
}
extern "C" __declspec(dllexport) bool GetWordWrap()
{
	return orbisLib->Settings->WordWrap;
}

extern "C" __declspec(dllexport) void SetWordWrap(bool Value)
{
	orbisLib->Settings->SetSettingbyName("WordWrap", Value);
}

#pragma endregion

#pragma endregion

extern "C"  __declspec(dllexport) void SetupCPP(bool bNoInstance)
{
	if (!bNoInstance)
	{
#ifndef _DEBUG
		EnableDebugLogs();
#endif

		printf("OrbisLib Loading...\n");
	}
		
	NoInstance = bNoInstance;
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
		m_hInstance = hModule;
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
		break;
    case DLL_PROCESS_DETACH:
		if(orbisLib)
			delete orbisLib;
        break;
    }
    return TRUE;
}

