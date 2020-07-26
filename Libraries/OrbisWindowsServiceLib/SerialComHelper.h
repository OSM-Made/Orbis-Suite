#pragma once

class SerialComHelper
{

public:
	char (*COMPortList)[11];
	int COMPortCount;
	bool SerialCOMPortListening;
	char ComPort[11];
	void(*SerialReadCallback)(const char* Data);

	bool UpdateCOMPortList();
	bool SelectCOMPort(const char* COMPort);
	static DWORD WINAPI SerialListenerThread(LPVOID lpParam);
	void StartListening();

	SerialComHelper(const char* DefaultComPort, void(*SerialReadCallback)(const char* Data));
	~SerialComHelper();

};