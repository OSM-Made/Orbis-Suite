#pragma once
#define MAX_CLIENTS 20
#define PORT_START 6920

class ServiceClient {
private:
	enum ClientCommands
	{
		CMD_CLIENT_CONNECT,
		CMD_CLIENT_DISCONNECT,
		CMD_CLIENT_PING
	};

	bool ServiceRunning = false;

public:
	struct ClientInfo_s
	{
		bool Used;
		unsigned short Port;
		int LastUpdateTime;
	}ClientInfo[MAX_CLIENTS];

	enum ClientInterrupts
	{
		INTERRUPT_STOP,
		INTERRUPT_PAUSE,
		INTERRUPT_RESUME,
		INTERRUPT_PROCESS_KILLED,
		INTERRUPT_TARGET_KILLED,
	};

	enum TargetMessageTypes {
		CMD_PRINT = 0,
		CMD_INTERRUPT,
		CMD_PROC_CHANGE,
		CMD_PROC_DETACH
	};

	struct CommandPacket_s
	{
		int CommandIndex;
		int Index;
	};

	static DWORD CommandClientThread(LPVOID lpParameter, SOCKET Client);
	SocketListener* CommandListener;

	static DWORD SocketAliveCheck(LPVOID ptr);

	ServiceClient(unsigned short CommandListenerPort);
	~ServiceClient();

	int AddClient();
	void RemoveClient(int index);

	bool SendClientPrint(ClientInfo_s ClientInfo, int Type, const char* Data, int length);
	bool SendClientInterrupt(ClientInfo_s ClientInfo, ClientInterrupts Interupt);
	bool SendProcChange(ClientInfo_s ClientInfo, const char* NewProc);
	bool SendProcDetach(ClientInfo_s ClientInfo);
};