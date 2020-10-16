#include "stdafx.h"
#include "SocketListener.h"

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
	this->ServerSocket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	//Set Sending and reciving time out to 1000 ms
	DWORD sock_timeout = 10000;
	setsockopt(this->ServerSocket, SOL_SOCKET, SO_SNDTIMEO, (char*)&sock_timeout, sizeof(sock_timeout));
	setsockopt(this->ServerSocket, SOL_SOCKET, SO_RCVTIMEO, (char*)&sock_timeout, sizeof(sock_timeout));

	//Bind our socket
	if (bind(this->ServerSocket, (sockaddr*)&addr, sizeof(addr)) == SOCKET_ERROR) {
		socketprint("Failed to bind Listener to Port %i\n", this->ListenPort);

		closesocket(this->ServerSocket);
		WSACleanup();

		DWORD Thr_Exit = 0;
		ExitThread(Thr_Exit);

		return 0;
	}

	//Start listening for incoming socket connections
	if (listen(this->ServerSocket, SOMAXCONN) == SOCKET_ERROR) {
		socketprint("[Error] Failed to start listen on Socket\n");

		closesocket(this->ServerSocket);
		WSACleanup();

		DWORD Thr_Exit = 0;
		ExitThread(Thr_Exit);

		return 0;
	}

	while (this->ServerRunning)
	{
		fd_set set;
		struct timeval timeout;
		int rv;
		FD_ZERO(&set); /* clear the set */
		FD_SET(this->ServerSocket, &set); /* add our file descriptor to the set */

		timeout.tv_sec = 2;
		timeout.tv_usec = 0;

		rv = select((int)this->ServerSocket + 1, &set, NULL, NULL, &timeout);
		if (rv == -1)
			goto Cleanup;
		else if (rv == 0)
		{
			if (!this->ServerRunning)
				goto Cleanup;
			continue;
		}
		else
		{
			//Accept incoming socket connections Clientaddr can be used if we want to know the client address
			struct sockaddr_in Clientaddr = { 0 };
			int addrlen = sizeof(sockaddr_in);
			SOCKET ClientSocket = accept(this->ServerSocket, (sockaddr*)&Clientaddr, &addrlen);

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
	}

Cleanup:
	//Clean Up
	closesocket(this->ServerSocket);
	WSACleanup();

	this->ThreadCleanedUp = true;

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
	socketprint("SocketListener Initialization!\n");

	//Store Our input varibales locally
	this->ClientCallBack = ClientCallBack;
	this->lpParameter = lpParameter;
	this->ServerRunning = true; //Used to signal thread to shut down
	this->ThreadCleanedUp = false; //Used to see when listen thread has closed.
	this->ListenPort = ListenPort;

	//Start Our Listeners Server thread
	DWORD hThreadID;
	hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)ThreadStartHack, this, 3, &hThreadID);
	ResumeThread(hThread);
	CloseHandle(hThread);
}

SocketListener::~SocketListener()
{
	socketprint("SocketListener Destruction!\n");

	//Signal Clean up
	this->ServerRunning = false;

	while (this->ThreadCleanedUp == false) {}

	socketprint("Destruction sucessful.\n");
}