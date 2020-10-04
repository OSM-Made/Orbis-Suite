#include "stdafx.hpp"
#include "Sockets.hpp"

bool Sockets::Connect() {
	struct sockaddr_in addr = { 0 };

	TIMEVAL Timeout = { 0 };
	Timeout.tv_sec = 1;
	Timeout.tv_usec = 0;

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

	if (!connect(Socket, (sockaddr*)&addr, sizeof(addr))) {
		printf("Failed to connect\n");
		return false;
	}

	iMode = 0;
	iResult = ioctlsocket(Socket, FIONBIO, &iMode);

	if (iResult != NO_ERROR) {
		printf("Failed to Set to Blocking Mode!\n");
		return false;
	}

	fd_set Write, Err;
	FD_ZERO(&Write);
	FD_ZERO(&Err);
	FD_SET(Socket, &Write);
	FD_SET(Socket, &Err);

	select(0, NULL, &Write, &Err, &Timeout);

	if (FD_ISSET(Socket, &Write))
		return true;

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

	//WSACleanup();
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
