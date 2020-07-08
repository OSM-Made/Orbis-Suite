#pragma once
#define MAX_CLIENTS 20
#define PORT_START 6920

class ServiceClient {
private:
	struct ClientInfo_s
	{
		bool Used;
		unsigned short Port;
		int LastUpdateTime; //Milliseconds  LastUpdateTime - GetTickCount() = TimeSinceUpdate
	}ClientInfo[MAX_CLIENTS];

public:
	enum ClientInterrupts
	{
		INTERRUPT_STOP,
		INTERRUPT_PAUSE,
		INTERRUPT_RESUME,
		INTERRUPT_PROCESS_KILLED,
		INTERRUPT_TARGET_KILLED,
	};

	enum ClientCommands {
		CMD_PRINT = 0,
		CMD_INTERRUPT,
		CMD_PROC_CHANGE,
		CMD_PROC_DETACH
	};

	ServiceClient();
	~ServiceClient();

	int AddClient();
	void RemoveClient(int index);

	bool SendClientPrint(ClientInfo_s ClientInfo, int Type, const char* Data, int length);
	bool SendClientInterrupt(ClientInfo_s ClientInfo, ClientInterrupts Interupt);
	bool SendProcChange(ClientInfo_s ClientInfo, const char* NewProc);
	bool SendProcDetach(ClientInfo_s ClientInfo);
};