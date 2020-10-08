#include "stdafx.h"
#include "sockets.h"

bool Sockets::Connect() {
	struct sockaddr_in addr = { 0 };

	WSADATA wsaData;
	WSAStartup(MAKEWORD(2, 2), &wsaData);

	Socket = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);

	if (Socket == INVALID_SOCKET) {
		WSACleanup();
		return false;
	}

	unsigned long iMode = 1;
	int iResult = 0;
	iResult = ioctlsocket(Socket, FIONBIO, &iMode);

	if (iResult != NO_ERROR) {
		printf("Failed to Set to NON Blocking Mode!\n");
		return false;
	}

	addr.sin_family = AF_INET;
	addr.sin_port = this->port;
	addr.sin_addr.s_addr = inet_addr(this->IP);

	DWORD sock_timeout = 1200;

	setsockopt(Socket, SOL_SOCKET, SO_SNDTIMEO, (char*)&sock_timeout, sizeof(sock_timeout));
	setsockopt(Socket, SOL_SOCKET, SO_RCVTIMEO, (char*)&sock_timeout, sizeof(sock_timeout));

	if (connect(Socket, (sockaddr*)&addr, sizeof(addr)) == SOCKET_ERROR)
	{
		if (WSAGetLastError() != WSAEWOULDBLOCK)
		{
			wprintf(L"connect function failed with error: %ld\n", WSAGetLastError());

			// connection failed
			closesocket(Socket);
			return false;
		}

		// connection pending

		fd_set setW, setE;

		FD_ZERO(&setW);
		FD_SET(Socket, &setW);
		FD_ZERO(&setE);
		FD_SET(Socket, &setE);

		timeval time_out = { 0 };
		time_out.tv_sec = 1;
		time_out.tv_usec = 0;

		int ret = select(0, NULL, &setW, &setE, &time_out);
		if (ret <= 0)
		{
			// select() failed or connection timed out
			closesocket(Socket);
			if (ret == 0)
				WSASetLastError(WSAETIMEDOUT);
			return false;
		}

		if (FD_ISSET(Socket, &setE))
		{
			// connection failed
			closesocket(Socket);
			return false;
		}
	}

	iMode = 0;
	iResult = ioctlsocket(Socket, FIONBIO, &iMode);

	if (iResult != NO_ERROR) {
		printf("Failed to Set to Blocking Mode!\n");
		return false;
	}

	return true;
}

Sockets::Sockets(const char* ConnectionAddr, unsigned short port) {
	memset(this->IP, 0, 16);
	strcpy_s(this->IP, ConnectionAddr);

	this->port = htons(port);
	this->hasConnectionBeenClosed = false;
}

Sockets::~Sockets() {
	if (!hasConnectionBeenClosed)
		Close();
}

void Sockets::Close() {
	hasConnectionBeenClosed = true;

	WSACleanup();
	shutdown(Socket, 2);
	closesocket(Socket);
}

bool Sockets::Send(const char* Data, int Length) {
	int Start = GetTickCount();

	char* CurrentPosition = (char*)Data;
	int DataLeft = Length;
	int SentStatus = 0;

	while (DataLeft > 0) {
		int DataChunkSize = min(1024 * 2, DataLeft);

		if (hasConnectionBeenClosed)
			return false;

		SentStatus = send(Socket, CurrentPosition, DataChunkSize, 0);
		if (SentStatus == -1 && errno != EWOULDBLOCK)
			break;

		DataLeft -= SentStatus;
		CurrentPosition += SentStatus;
	}

	if (SentStatus == -1)
		return false;

	return true;
}

bool Sockets::Receive(char* Data, int Size) {
	int Start = GetTickCount();

	char* CurrentPosition = (char*)Data;
	int DataLeft = Size;
	int ReceiveStatus = 0;

	while (DataLeft > 0) {
		int DataChunkSize = min(1024 * 2, DataLeft);

		if (hasConnectionBeenClosed)
			return false;

		ReceiveStatus = recv(Socket, CurrentPosition, DataChunkSize, 0);
		if (ReceiveStatus == -1 && errno != EWOULDBLOCK)
			break;

		CurrentPosition += ReceiveStatus;
		DataLeft -= ReceiveStatus;
	}

	if (ReceiveStatus == -1)
		return false;

	return true;
}
