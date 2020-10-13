#include "stdafx.h"
#include "ServiceTargetWatcher.h"

void ServiceTargetWatcher::WatcherChildThread(int index)
{
	orbisLib->TargetManagement->UpdateTargetExtInfo(index);

	if (orbisLib->TargetManagement->Targets[index].Available)
	{
		if (LastTargetValue[index] == false)
		{
			printf("Target: %s Available.\n", orbisLib->TargetManagement->Targets[index].Name);

			//Set up packet to send.
			TargetCommandPacket_s TargetCommandPacket;
			TargetCommandPacket.CommandIndex = CMD_TARGET_AVAILABILITY;
			TargetCommandPacket.Target.Available = true;
			strcpy_s(TargetCommandPacket.Target.TargetName, 0x100, orbisLib->TargetManagement->Targets[index].Name);

			//Forward the packet to all the connected children processes.
			Client->ForwardPacket(&TargetCommandPacket);

			LastTargetValue[index] = true;
		}
	}
	else
	{
		if (LastTargetValue[index] == true)
		{
			printf("Target: %s no longer Available.\n", orbisLib->TargetManagement->Targets[index].Name);

			//Set up packet to send.
			TargetCommandPacket_s TargetCommandPacket;
			TargetCommandPacket.CommandIndex = CMD_TARGET_AVAILABILITY;
			TargetCommandPacket.Target.Available = false;
			strcpy_s(TargetCommandPacket.Target.TargetName, 0x100, orbisLib->TargetManagement->Targets[index].Name);

			//Forward the packet to all the connected children processes.
			Client->ForwardPacket(&TargetCommandPacket);

			LastTargetValue[index] = false;
		}
	}
}

DWORD WINAPI StartChildThread(LPVOID ptr)
{
	ChildThreadParam* TP = (ChildThreadParam*)ptr;

	TP->TargetWatcher->WatcherChildThread(TP->Index);

	free(TP);

	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

void ServiceTargetWatcher::WatcherThread()
{

	while (this->ServiceRunning)
	{
		int TargetCount = orbisLib->TargetManagement->GetTargetCount();
		if (TargetCount == 0)
		{
			Sleep(1000);
			continue;
		}

		for (int i = 0; i < TargetCount; i++)
		{
			ChildThreadParam* ThreadParam = (ChildThreadParam*)malloc(sizeof(ChildThreadParam));
			ThreadParam->TargetWatcher = this;
			ThreadParam->Index = i;

			CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)StartChildThread, ThreadParam, 3, NULL);
		}

		Sleep(1000);
	}
}

DWORD WINAPI StartWatcherThread(LPVOID ptr)
{
	((ServiceTargetWatcher*)ptr)->WatcherThread();

	DWORD Thr_Exit = 0;
	ExitThread(Thr_Exit);
}

ServiceTargetWatcher::ServiceTargetWatcher()
{
	this->ServiceRunning = true;

	CreateThread(NULL, NULL, (LPTHREAD_START_ROUTINE)StartWatcherThread, this, 3, NULL);
}

ServiceTargetWatcher::~ServiceTargetWatcher()
{
	this->ServiceRunning = false;
}