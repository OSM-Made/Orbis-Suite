#include "stdafx.hpp"
#include "SocketListener.hpp"

DWORD WINAPI ClientThreadStart(LPVOID ptr)
{
	((SocketListener*)ptr)->ClientCallBack(((SocketListener*)ptr)->lpParameter, ((SocketListener*)ptr)->ClientSocket);

	closesocket(((SocketListener*)ptr)->ClientSocket);

	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

DWORD WINAPI SocketListener::ListenerThread() {
	//Initialize socket addr struct
	struct sockaddr_in addr = { 0 };
	addr.sin_family = AF_INET;
	addr.sin_addr.s_addr = INADDR_ANY; //Any incoming address
	addr.sin_port = htons(this->ListenPort); //Our desired listen port

	//WSA Startup
	WSADATA wsaData;
	WSAStartup(MAKEWORD(2, 2), &wsaData);

	//Make new TCP Socket
	SOCKET Server = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	//Set Sending and reciving time out to 1000 ms
	DWORD sock_timeout = 10000;
	setsockopt(Server, SOL_SOCKET, SO_SNDTIMEO, (char*)&sock_timeout, sizeof(sock_timeout));
	setsockopt(Server, SOL_SOCKET, SO_RCVTIMEO, (char*)&sock_timeout, sizeof(sock_timeout));

	//Bind our socket
	if (bind(Server, (sockaddr*)&addr, sizeof(addr)) == SOCKET_ERROR) {
		printf("Failed to bind Listener to Port %i\n", this->ListenPort);

		closesocket(Server);
		WSACleanup();

		DWORD Thr_Exit = 0;
		ExitThread(Thr_Exit);

		return 0;
	}

	//Start listening for incoming socket connections
	if (listen(Server, SOMAXCONN) == SOCKET_ERROR) {
		printf("[Error] Failed to start listen on Socket\n");

		closesocket(Server);
		WSACleanup();

		DWORD Thr_Exit = 0;
		ExitThread(Thr_Exit);

		return 0;
	}

	while (this->ServerRunning)
	{
		//Accept incoming socket connections Clientaddr can be used if we want to know the client address
		struct sockaddr_in Clientaddr = { 0 };
		int addrlen = sizeof(sockaddr_in);
		SOCKET ClientSocket = accept(Server, (sockaddr*)&Clientaddr, &addrlen);

		if (ClientSocket)
		{
			//Store ClientSocket
			this->ClientSocket = ClientSocket;

			//Create thread to handle our socket client
			DWORD hThreadID;
			HANDLE hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ClientThreadStart, (LPVOID)this, 3, &hThreadID);
			ResumeThread(hThread);
			CloseHandle(hThread);

			//Reset our local socket variable for next client
			ClientSocket = -1;
		}
	}

	//Clean Up
	closesocket(Server);
	WSACleanup();

	//Exit Thread
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

DWORD WINAPI ThreadStartHack(LPVOID ptr)
{
	return ((SocketListener*)ptr)->ListenerThread();
}

SocketListener::SocketListener(VOID(*ClientCallBack)(LPVOID, SOCKET), LPVOID lpParameter, unsigned short ListenPort)
{
	printf("SocketListener Initialization!\n");

	//Store Our input varibales locally
	this->ClientCallBack = ClientCallBack;
	this->lpParameter = lpParameter;
	this->ServerRunning = true; //Used to signal thread to shut down
	this->ListenPort = ListenPort;

	//Start Our Listeners Server thread
	DWORD hThreadID;
	HANDLE hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ThreadStartHack, this, 3, &hThreadID);
	ResumeThread(hThread);
	CloseHandle(hThread);
}

SocketListener::~SocketListener()
{
	printf("SocketListener Destruction!\n");

	//Signal Clean up
	this->ServerRunning = false;
}