#pragma once
#include "Export.hpp"

class OrbisLib;

class EXPORT OrbisProc
{
private:
	OrbisLib* orbisLib;

public:
	OrbisProc(OrbisLib* OrbisLib);
	~OrbisProc();

	int GetList(char* IPAddr, int32_t* ProcCount, char* List);
	int Attach(char* IPAddr, char* ProcName);
	int Detach(char* IPAddr, char* ProcName);
	int GetCurrent(char* IPAddr, RESP_Proc* Out);
	int Read(char* IPAddr, uint64_t Address, size_t Len, char* Data);
	int Write(char* IPAddr, uint64_t Address, size_t Len, char* Data);
	int Kill(char* IPAddr, char* ProcName);
	int LoadELF(char* IPAddr, char* Buffer, size_t Len);
	//TODO: Implement RPC Call

	//Libraries
	int LoadSPRX(char* IPAddr, char* Path, int32_t Flags);
	int UnloadSPRX(char* IPAddr, int32_t Handle, int32_t Flags);
	int UnloadSPRX(char* IPAddr, char* Name, int32_t Flags);
	int ReloadSPRX(char* IPAddr, char* Name, int32_t Flags);
	int ReloadSPRX(char* IPAddr, int32_t Handle, int32_t Flags);
	int GetLibraryList(char* IPAddr, int32_t* LibraryCount, char* Out);
};