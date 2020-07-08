#pragma once

class ServiceLib {

public:
	ServiceClient* Client;
	bool ServiceRunning = false;

	char CurrentTarget[512];
	char CurrentProcess[512];

	static DWORD CommandClientThread(LPVOID lpParameter, SOCKET Client);
	SocketListener* CommandListener;
	SocketListener* TargetListener;
	SocketListener* LogListener;

	ServiceLib(unsigned short CommandListenerPort = 6901, unsigned short TargetListenerPort = 6902);
	~ServiceLib();

private:
	enum ClientCommands
	{
		//Target
		CMD_GET_CURRENT_TARGET = 1,
		CMD_SET_CURRENT_TARGET,
		CMD_CONNECT_TO_TARGET,
		CMD_DISCONNECT_FROM_TARGET,
		CMD_GET_CURRENT_PROCESS,
		CMD_SET_CURRENT_PROCESS,

		//Target Management
		CMD_SET_DEFAULT_TARGET,
		CMD_GET_DEFAULT_TARGET,
		CMD_ADD_NEW_TARGET,
		CMD_DELETE_TARGET,
		CMD_UPDATE_TARGET,
		CMD_GET_TARGET_INFO,
		CMD_GET_TARGET_LIST,

		//Session
		CMD_CLIENT_CONNECT,
		CMD_CLIENT_DISCONNECT,
		CMD_CLIENT_PING
	};

	struct SetCurrentTargetPacket_s
	{
		char TargetName[512];
	};

	struct SetCurrentProcessPacket_s
	{
		char ProcessName[512];
	};

	struct NewTargetPacket_s
	{
		char TargetName[512];
		char Firmware[10];
		char IPAddress[20];
	};

	struct UpdateTargetPacket_s
	{
		char OldTargetName[512];
		char NewTargetName[512];
		char NewFirmware[10];
		char NewIPAddress[20];
	};

	struct CommandPacket_s
	{
		int CommandNumber;
		union
		{
			SetCurrentTargetPacket_s SetCurrentTargetPacket;
			SetCurrentProcessPacket_s SetCurrentProcessPacket;
			NewTargetPacket_s NewTargetPacket;
			UpdateTargetPacket_s UpdateTargetPacket;
		};
	};
};