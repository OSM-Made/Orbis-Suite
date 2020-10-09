#pragma once

class SocketListener
{
private:
	HANDLE hThread;
	SOCKET ServerSocket;
	bool ServerRunning;
	bool ThreadCleanedUp;
	unsigned short ListenPort;

public:
	SOCKET ClientSocket;
	LPVOID lpParameter;
	VOID(*ClientCallBack)(LPVOID lpParameter, SOCKET);
	DWORD WINAPI ListenerThread();

	SocketListener(VOID(*ClientCallBack)(LPVOID lpParameter, SOCKET), LPVOID lpParameter, unsigned short ListenPort);
	~SocketListener();
};