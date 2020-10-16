#pragma once
#include "stdafx.h"

class ServiceTargetWatcher
{
private:
	bool ServiceRunning;
	bool LastTargetValue[20];
	int LastTargetCount;

public:
	void WatcherChildThread(int index);
	void WatcherThread();

	ServiceTargetWatcher();
	~ServiceTargetWatcher();
};

struct ChildThreadParam
{
	ServiceTargetWatcher* TargetWatcher;
	int Index;
};