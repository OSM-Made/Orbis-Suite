#include "stdafx.h"
#include "SerialComHelper.h"

int SerialComHelper::GetCOMPortCount()
{

}

void SerialComHelper::GetCOMPorts(char* Buffer, int Count)
{

}

SerialComHelper::SerialComHelper(const char* DefaultComPort, void(*SerialReadCallback)(const char* Data))
{
	printf("SerialComHelper Initialization!\n");

	//Set Default Com Port
	strcpy_s((char*)this->ComPort, 0x10, DefaultComPort);

	//Set a call back to call when data is read
	this->SerialReadCallback = SerialReadCallback;
}

SerialComHelper::~SerialComHelper()
{
	printf("SerialComHelper Destruction...\n");

}