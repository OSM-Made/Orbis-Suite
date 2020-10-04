#pragma once

enum API_COMMANDS
{
	NULL_PACKET = 0,
	API_TEST_COMMS,

	/* ####### Proc functions ####### */
	PROC_START,
	API_PROC_GET_LIST,
	API_PROC_ATTACH,
	API_PROC_DETACH,
	API_PROC_GET_CURRENT,
	API_PROC_READ,
	API_PROC_WRITE,
	API_PROC_KILL,
	API_PROC_LOAD_ELF,
	API_PROC_CALL,

	/* Remote Library functions */
	API_PROC_LOAD_SPRX,
	API_PROC_UNLOAD_SPRX,
	API_PROC_RELOAD_SPRX_NAME,
	API_PROC_RELOAD_SPRX_HANDLE,
	API_PROC_MODULE_LIST,
	PROC_END,
	/* ############################## */


	/* ##### Debugger functions ##### */
	DBG_START,
	API_DBG_START, /* Debugger attach to target */
	API_DBG_STOP, /* Debugger detach from target */
	API_DBG_BREAK,
	API_DBG_RESUME,
	API_DBG_SIGNAL,
	API_DBG_STEP,
	API_DBG_STEP_OVER,
	API_DBG_STEP_OUT,
	API_DBG_GET_CALLSTACK,
	API_DBG_GET_REG,
	API_DBG_SET_REG,
	API_DBG_GET_FREG,
	API_DBG_SET_FREG,
	API_DBG_GET_DBGREG,
	API_DBG_SET_DBGREG,

	/* Thread Management */
	API_DBG_THREAD_LIST,
	API_DBG_THREAD_STOP,
	API_DBG_THREAD_RESUME,

	/* Breakpoint functions */
	API_DBG_BREAKPOINT_GETFREE,
	API_DBG_BREAKPOINT_SET,
	API_DBG_BREAKPOINT_UPDATE,
	API_DBG_BREAKPOINT_REMOVE,
	API_DBG_BREAKPOINT_GETINFO,
	API_DBG_BREAKPOINT_LIST,

	/* Watchpoint functions */
	API_DBG_WATCHPOINT_SET,
	API_DBG_WATCHPOINT_UPDATE,
	API_DBG_WATCHPOINT_REMOVE,
	API_DBG_WATCHPOINT_GETINFO,
	API_DBG_WATCHPOINT_LIST,
	DBG_END,
	/* ############################## */

	/* ###### Kernel functions ###### */
	KERN_START,
	API_KERN_BASE,
	API_KERN_READ,
	API_KERN_WRITE,
	KERN_END,
	/* ############################## */

	/* ###### Target functions ###### */
	TARGET_START,
	API_TARGET_INFO,
	API_TARGET_SHUTDOWN,
	API_TARGET_REBOOT,
	API_TARGET_NOTIFY,
	API_TARGET_BEEP,
	API_TARGET_SET_LED,
	API_TARGET_GET_LED,
	API_TARGET_DUMP_PROC,
	//API_TARGET_LOAD_VSH_MODULE
	TARGET_END,
	/* ############################## */
};

enum API_ERRORS
{
	API_OK = 0,
	API_ERROR_NOT_CONNECTED,
	API_ERROR_FAILED_TO_CONNNECT,
	API_ERROR_NOT_REACHABLE,
	API_ERROR_NOT_ATTACHED,
	API_ERROR_LOST_PROC,

	API_ERROR_FAIL,
	API_ERROR_INVALID_ADDRESS,

	//Debugger
	API_ERROR_PROC_RUNNING,
	API_ERROR_DEBUGGER_NOT_ATTACHED,
};

struct API_Packet_s
{
	//int8_t PacketVersion;
	API_COMMANDS cmd;
	char ProcName[0x20];
	union
	{
		struct
		{
			uint64_t Address;
			size_t len;
		}PROC_RW;
		struct
		{
			char ModuleDir[0x100];
			int hModule;
			int Flags;
		}PROC_SPRX;
		struct
		{
			int32_t Index;
			uint64_t Address;
			bool Enable;

		}Breakpoint;
		struct
		{
			int MessageType;
			char Message[100];
		}Target_Notify;
		struct
		{
			int Count;
		}Target_Beep;
	};
};

struct RESP_ProcList
{
	unsigned int ProcessID; //0x00
	unsigned int Attached; //0x04
	char ProcName[32]; //0x08
	char TitleID[10]; //0x28
};

struct RESP_CurrentProc
{
	unsigned int ProcessID; //0x00
	char ProcName[32]; //0x04
	char TitleID[10]; //0x24
	uint64_t TextSegmentBase;
	uint64_t TextSegmentLen;
	uint64_t DataSegmentBase;
	uint64_t DataSegmentLen;
};

struct RESP_ModuleList
{
	char mName[0x24]; //0x00
	char mPath[0x100]; //0x24
	int mHandle; //0x124
	uint64_t mTextSegmentBase; //0x128
	uint64_t mTextSegmentLen; //0x130
	uint64_t mDataSegmentBase; //0x138
	uint64_t mDataSegmentLen; //0x140
};

enum ConsoleTypes
{
	UNK,
	DIAG, //0x80
	DEVKIT, //0x81
	TESTKIT, //0x82
	RETAIL, //0x83 -> 0x8F
	KRATOS, //0xA0 IMPOSSIBLE??
};

struct RESP_TargetInfo
{
	int32_t SDKVersion;
	int32_t SoftwareVersion;
	int32_t CPUTemp;
	char CurrentTitleID[10];
	char ConsoleName[100];
	char IDPS[16];
	char PSID[16];
	int32_t ConsoleType;
};

struct DB_TargetInfo
{
	char Name[0x100];
	char IPAddr[16];
	int Firmware;
};