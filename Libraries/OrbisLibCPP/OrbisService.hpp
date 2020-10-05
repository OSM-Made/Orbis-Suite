#pragma once
class OrbisLib;

#define PORT_COMMANDSERVER 6902
#define MAX_CLIENTS 20
#define PORT_START 6920

enum TargetCommands
{
	CMD_PRINT,

	CMD_INTERCEPT,
	CMD_CONTINUE,

	CMD_PROC_DIE,
	CMD_PROC_ATTACH,
	CMD_PROC_DETACH,

	CMD_TARGET_SUSPEND,
	CMD_TARGET_RESUME,
	CMD_TARGET_SHUTDOWN,

	CMD_DB_TOUCHED,

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
	union
	{
		char ProcName[0x20];
		struct
		{
			int Type;
			int Len;
		}Print;
		struct
		{
			int Reason;
			reg Registers;
		}Break;
	};
};

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
	void SendHeartBeat();

	void HandlePrint(TargetCommandPacket_s* Packet, SOCKET Socket);

public:
	OrbisService(OrbisLib* orbisLib);
	~OrbisService();
};