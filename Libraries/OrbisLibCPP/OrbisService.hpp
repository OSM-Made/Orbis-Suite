#pragma once
class OrbisLib;

#define PORT_COMMANDSERVER 6902
#define MAX_CLIENTS 20
#define PORT_START 6920

enum TargetCommands
{
	//Print Server
	CMD_PRINT = 1,

	//Debugging
	CMD_INTERCEPT,
	CMD_CONTINUE,

	//Proc States
	CMD_PROC_DIE,
	CMD_PROC_ATTACH,
	CMD_PROC_DETACH,

	//Target State
	CMD_TARGET_SUSPEND,
	CMD_TARGET_RESUME,
	CMD_TARGET_SHUTDOWN,
	CMD_TARGET_NEWTITLE,

	//DB Watcher
	CMD_DB_TOUCHED,

	//Target Availability
	CMD_TARGET_AVAILABILITY,
};

enum PrintType
{
	PT_SOCK,
	PT_SERIAL
};

typedef int register_t;
struct reg {
	register_t	r_r15;
	register_t	r_r14;
	register_t	r_r13;
	register_t	r_r12;
	register_t	r_r11;
	register_t	r_r10;
	register_t	r_r9;
	register_t	r_r8;
	register_t	r_rdi;
	register_t	r_rsi;
	register_t	r_rbp;
	register_t	r_rbx;
	register_t	r_rdx;
	register_t	r_rcx;
	register_t	r_rax;
	uint32_t	r_trapno;
	uint16_t	r_fs;
	uint16_t	r_gs;
	uint32_t	r_err;
	uint16_t	r_es;
	uint16_t	r_ds;
	register_t	r_rip;
	register_t	r_cs;
	register_t	r_rflags;
	register_t	r_rsp;
	register_t	r_ss;
};

struct TargetCommandPacket_s
{
	int CommandIndex;
	char IPAddr[16];
	union
	{
		char ProcName[0x20];
		struct
		{
			char TitleID[0x20];
		}TitleChange;
		struct
		{
			char Sender[0x100];
			char Data[0x400];
			int Type;
		}Print;
		struct
		{
			int Reason;
			reg Registers;
		}Break;
		struct
		{
			bool Available;
			char TargetName[0x100];
		}Target;
	};
};

typedef void(*Target_Print_Callback)(char* IPAddr, char* Sender, int Type, char* Data);
typedef void(*Proc_Intercept_Callback)(char* IPAddr, int Reason, reg* Registers);
typedef void(*Proc_Continue_Callback)(char* IPAddr);
typedef void(*Proc_Die_Callback)(char* IPAddr);
typedef void(*Proc_Attach_Callback)(char* IPAddr, char* NewProc);
typedef void(*Proc_Detach_Callback)(char* IPAddr);
typedef void(*Target_Suspend_Callback)(char* IPAddr);
typedef void(*Target_Resume_Callback)(char* IPAddr);
typedef void(*Target_Shutdown_Callback)(char* IPAddr);
typedef void(*Target_NewTitle_Callback)(char* IPAddr, char* NewTitle);
typedef void(*DB_Touched_Callback)();
typedef void(*Target_Availability_Callback)(bool Available, char* NewTargetData);


class OrbisService
{
private:
	OrbisLib* orbisLib;
	bool IsConnectedtoService;
	bool IsRunning;
	int ClientIndex;

	SocketListener* ServiceListener;
	static VOID ServiceCallback(LPVOID lpParameter, SOCKET Socket);
	static DWORD WINAPI HeartBeatThread(LPVOID Params);

	bool Connect();
	void Disconnect();
	bool SendHeartBeat();

public:
	//Call Backs
	Target_Print_Callback Target_Print;
	Proc_Intercept_Callback Proc_Intercept;
	Proc_Continue_Callback Proc_Continue;
	Proc_Die_Callback Proc_Die;
	Proc_Attach_Callback Proc_Attach;
	Proc_Detach_Callback Proc_Detach;
	Target_Suspend_Callback Target_Suspend;
	Target_Resume_Callback Target_Resume;
	Target_Shutdown_Callback Target_Shutdown;
	Target_NewTitle_Callback Target_NewTitle;
	DB_Touched_Callback DB_Touched;
	Target_Availability_Callback Target_Availability;

	OrbisService(OrbisLib* orbisLib);
	~OrbisService();
};