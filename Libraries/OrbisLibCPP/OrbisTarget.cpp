#include "stdafx.hpp"
#include "OrbisDef.hpp"
#include "OrbisLib.hpp"
#include "OrbisTarget.hpp"

OrbisTarget::OrbisTarget(OrbisLib* orbisLib)
{
	this->orbisLib = orbisLib;
}

OrbisTarget::~OrbisTarget()
{

}

int OrbisTarget::SendPayload(char* IPAddr, int Firmware, short Port)
{
	Sockets* Socket = new Sockets(IPAddr, Port);

	if (!Socket->Connect()) {
		Socket->Close();
		return API_ERROR_FAILED_TO_CONNNECT;
	}

	//Get the file path for the correct payload.
	char FilePath[MAX_PATH];
	sprintf_s(FilePath, "%s\\Orbis Suite\\OrbisLib-%d.bin", orbisLib->TargetManagement->ProgramDataBuffer, Firmware);

	//Get the size of the file and allocate heap space to read it.
	int64_t FileSize = FileIO::FileSize(FilePath);
	char* PayloadData = (char*)malloc(FileSize);

	//Read the payload data from the disk.
	if (!FileIO::FileRead(FilePath, PayloadData, FileSize))
	{
		printf("Failed to read Payload from HDD. %s\n", FilePath);

		Socket->Close();
		free(PayloadData);

		return API_ERROR_FAIL;
	}

	Sleep(2000);

	//Send the payload.
	if (!Socket->Send(PayloadData, (int)FileSize))
	{
		printf("Failed to Send Payload.\n");

		Socket->Close();
		free(PayloadData);

		return API_ERROR_FAIL;
	}

	//clean up and return success.
	Socket->Close();
	free(PayloadData);

	return API_OK;
}

int OrbisTarget::GetInfo(char* IPAddr, RESP_TargetInfo* TargetInfo)
{
	int Status = API_OK;

	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_INFO;

	Sockets* Sock;
	Status = orbisLib->API->CallLong(&Sock, IPAddr, &API_Packet);

	if (Status)
		return Status;

	if (!Sock->Receive((char*)TargetInfo, sizeof(RESP_TargetInfo)))
	{
		orbisLib->API->FinishCall(&Sock);

		return API_ERROR_FAIL;
	}

	char test[sizeof(API_Packet_s)];

	orbisLib->API->FinishCall(&Sock);

	return API_OK;
}

int OrbisTarget::Shutdown(char* IPAddr)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_SHUTDOWN;

	return orbisLib->API->Call(IPAddr, &API_Packet);
}

int OrbisTarget::Reboot(char* IPAddr)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_REBOOT;

	return orbisLib->API->Call(IPAddr, &API_Packet);
}

int OrbisTarget::Suspend(char* IPAddr)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_SHUTDOWN;

	return orbisLib->API->Call(IPAddr, &API_Packet);
}

int OrbisTarget::Notify(char* IPAddr, int Type, const char* Message)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_NOTIFY;
	API_Packet.Target_Notify.MessageType = Type;
	strcpy_s(API_Packet.Target_Notify.Message, Message);

	return orbisLib->API->Call(IPAddr, &API_Packet);
}

int OrbisTarget::Beep(char* IPAddr, int Count)
{
	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_BEEP;
	API_Packet.Target_Beep.Count = Count;

	return orbisLib->API->Call(IPAddr, &API_Packet);
}

int OrbisTarget::SetLED(char* IPAddr, char R, char G, char B, char A)
{
	return API_ERROR_FAIL;
}

int OrbisTarget::GetLED(char* IPAddr, char* R, char* G, char* B, char* A)
{
	return API_ERROR_FAIL;
}

int OrbisTarget::DumpProcess(char* IPAddr, const char* ProcName, uint64_t* Size, char* Out)
{
	int Status = API_OK;

	API_Packet_s API_Packet;
	memset(&API_Packet, 0, sizeof(API_Packet_s));
	API_Packet.cmd = API_TARGET_DUMP_PROC;

	Sockets* Sock;
	Status = orbisLib->API->CallLong(&Sock, IPAddr, &API_Packet);

	if (Status)
		return Status;

	//Recieve the size of the dumped module.
	if (!Sock->Receive((char*)Size, sizeof(uint64_t)))
	{
		orbisLib->API->FinishCall(&Sock);

		return API_ERROR_FAIL;
	}

	//Recieve the data.
	if (!Sock->Receive((char*)Out, (int)*Size))
	{
		orbisLib->API->FinishCall(&Sock);

		return API_ERROR_FAIL;
	}

	//Finish up the API call and clean up.
	orbisLib->API->FinishCall(&Sock);

	//Return our sucess 
	return API_OK;
}