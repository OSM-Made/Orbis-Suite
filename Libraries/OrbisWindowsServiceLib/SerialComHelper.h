#pragma once

class SerialComHelper
{

public:
	const char* ComPort;
	void(*SerialReadCallback)(const char* Data);

	int GetCOMPortCount();
	void GetCOMPorts(char* Buffer, int Count);

	SerialComHelper(const char* DefaultComPort, void(*SerialReadCallback)(const char* Data));
	~SerialComHelper();

};