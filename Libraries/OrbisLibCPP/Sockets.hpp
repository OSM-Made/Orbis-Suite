#pragma once

class Sockets {
public:
	char IP[16];
	SOCKET Socket;

	Sockets(const char* ConnectReceiveionAddr, unsigned short port);
	Sockets(SOCKET Socket);
	~Sockets();

	void Close();
	bool Connect();
	bool Send(const char* Data, int Length);
	bool Receive(char* Data, int Size);

private:
	unsigned short port;
	bool hasConnectionBeenClosed;
};
