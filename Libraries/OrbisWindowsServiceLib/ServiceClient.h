#pragma once
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
	CMD_TARGET_SHUTDOWN
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

enum TRAP_FAULTS
{
	T_RESADFLT = 0,
	T_PRIVINFLT = 1,
	T_RESOPFLT = 2,
	T_BPTFLT = 3,
	T_SYSCALL = 5,
	T_ARITHTRAP = 6,
	T_ASTFLT = 7,
	T_SEGFLT = 8,
	T_PROTFLT = 9,
	T_TRCTRAP = 10,
	T_PAGEFLT = 12,
	T_TABLEFLT = 13,
	T_ALIGNFLT = 14,
	T_KSPNOTVAL = 15,
	T_BUSERR = 16,
	T_KDBTRAP = 17,

	T_DIVIDE = 18,
	T_NMI = 19,
	T_OFLOW = 20,
	T_BOUND = 21,
	T_DNA = 22,
	T_DOUBLEFLT = 23,
	T_FPOPFLT = 24,
	T_TSSFLT = 25,
	T_SEGNPFLT = 26,
	T_STKFLT = 27,
	T_RESERVED = 28,
};

class ServiceClient {
private:
	struct ClientInfo_s
	{
		bool Used;
		unsigned short Port;
		int LastUpdateTime;
	}ClientInfo[MAX_CLIENTS];

	enum ClientCommands
	{
		CMD_CLIENT_CONNECT,
		CMD_CLIENT_DISCONNECT,
		CMD_CLIENT_PING
	};

	struct CommandPacket_s
	{
		int CommandIndex;
		int Index;
	};

	bool ServiceRunning = false;

public:
	static DWORD CommandClientThread(LPVOID lpParameter, SOCKET Client);
	SocketListener* CommandListener;
	static DWORD SocketAliveCheck(LPVOID ptr);

	ServiceClient(unsigned short CommandListenerPort);
	~ServiceClient();

	int AddClient();
	void RemoveClient(int index);

	void ForwardPacket(TargetCommandPacket_s* TargetCommandPackets);
};