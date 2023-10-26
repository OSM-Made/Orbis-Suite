#include "stdafx.h"
#include "SystemMonitor.h"
#include <SystemStateMgr.h>

void SystemMonitor::Run()
{
	while (true)
	{
		switch ((SystemState)sceSystemStateMgrGetCurrentState())
		{
		default:
			break;

			case SHUTDOWN_ON_GOING:
				Logger::Info("Console is shutting down! API exiting...\n");
				return;

			case POWER_SAVING:
			case SUSPEND_ON_GOING:
			case MAIN_ON_STANDBY:
				Logger::Info("Going to sleep!\n");
				break;

			case INITIALIZING:
				Logger::Info("Waking up!\n");
				break;
		}

		// Sleep for 5s before we check our state again.
		sceKernelSleep(5);
	}
}