#include "stdafx.h"
#include "SerialComHelper.h"

bool SerialComHelper::UpdateCOMPortList()
{
	DWORD nValues = 0, nMaxValueNameLen = 0, nMaxValueLen = 0, dwType = 0, dwIndex = 0;
	HKEY hKey = NULL;
	CHAR DeviceName[1000];

	//Open Reg Key for serial com ports list
	LSTATUS lResult = RegOpenKeyEx(HKEY_LOCAL_MACHINE, L"HARDWARE\\DEVICEMAP\\SERIALCOMM", 0, KEY_READ, &hKey);
	if (ERROR_SUCCESS != lResult)
	{
		printf("Failed to open key \'HARDWARE\\DEVICEMAP\\SERIALCOMM\' \n");
		return false;
	}

	//Query for some important info such as the number of com ports and the name sizes
	lResult = RegQueryInfoKey(hKey, NULL, NULL, NULL, NULL, NULL, NULL, &nValues, &nMaxValueNameLen, &nMaxValueLen, NULL, NULL);

	if (ERROR_SUCCESS != lResult)
	{
		printf("Failed to RegQueryInfoKey()\n");
		RegCloseKey(hKey);
		return false;
	}

	//Free previous list
	if (this->COMPortList)
		free(this->COMPortList);

	//Allocate space to store list
	this->COMPortList = new char[nValues][11]();
	this->COMPortCount = nValues;

	for (DWORD dwIndex = 0; dwIndex < nValues; ++dwIndex)
	{
		dwType = 0;
		nMaxValueNameLen += sizeof(CHAR); //Add room for null terminator
		nMaxValueLen += sizeof(CHAR);

		//Get Reg values for index and store names into our list
		lResult = RegEnumValueA(hKey, dwIndex, DeviceName, &nMaxValueNameLen, NULL, &dwType, (LPBYTE)this->COMPortList[dwIndex], &nMaxValueLen);

		if (ERROR_SUCCESS != lResult || REG_SZ != dwType) {
			printf("SerialPortEnumerator::Init() : can't process registry value, index: %d\n", dwIndex);
			continue;
		}
	}

	//Clean Up
	RegCloseKey(hKey);

	//Return Success
	return true;
}

bool SerialComHelper::SelectCOMPort(const char* COMPort)
{
	CHAR lpTargetPath[5000];
	if (!QueryDosDeviceA(COMPort, (LPSTR)lpTargetPath, 5000))
	{
		printf("Failed to connect to COM Port \"%s\"", COMPort);
		return false;
	}

	strcpy_s((char*)this->ComPort, 11, COMPort);

	return true;
}

DWORD WINAPI SerialComHelper::SerialListenerThread(LPVOID lpParam)
{
	SerialComHelper* serialComHelper = (SerialComHelper*)lpParam;

	HANDLE m_hCommPort = CreateFileA(serialComHelper->ComPort, GENERIC_READ | GENERIC_WRITE, 0, 0, OPEN_EXISTING, FILE_FLAG_OVERLAPPED, 0);



	while (serialComHelper->SerialCOMPortListening)
	{

	}

	//Exit Thread
	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

void SerialComHelper::StartListening()
{
	if (this->ComPort == NULL || !strcmp(this->ComPort, ""))
	{
		printf("Error COM Port name is NULL\n");
		return;
	}

	CHAR lpTargetPath[5000];
	if (!QueryDosDeviceA(this->ComPort, (LPSTR)lpTargetPath, 5000))
	{
		printf("Failed to connect to COM Port \"%s\"", ComPort);
		return;
	}

	this->SerialCOMPortListening = true;

	//Start a thread to listen for data and call our call back when we recieve data
	DWORD hThreadID;
	HANDLE hThread = CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)SerialListenerThread, this, 3, &hThreadID);
	ResumeThread(hThread);
	CloseHandle(hThread);
}

SerialComHelper::SerialComHelper(const char* DefaultComPort, void(*SerialReadCallback)(const char* Data))
{
	printf("SerialComHelper Initialization!\n");

	//Set Default Com Port
	this->SelectCOMPort(DefaultComPort);

	//Set a call back to call when data is read
	this->SerialReadCallback = SerialReadCallback;

	//Populate COM Port List
	this->UpdateCOMPortList();


}

SerialComHelper::~SerialComHelper()
{
	printf("SerialComHelper Destruction...\n");

	if (COMPortList)
		free(COMPortList);
}