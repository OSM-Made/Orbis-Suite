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
	LPVOID lpParameter;
	VOID(*ClientCallBack)(LPVOID lpParameter, Sockets*);
	DWORD WINAPI ListenerThread();

	SocketListener(VOID(*ClientCallBack)(LPVOID lpParameter, Sockets*), LPVOID lpParameter, unsigned short ListenPort);
	~SocketListener();
};