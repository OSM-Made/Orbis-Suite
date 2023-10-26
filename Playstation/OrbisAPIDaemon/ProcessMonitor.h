#pragma once

class ProcessMonitor
{
public:
	ProcessMonitor(int pid);
	~ProcessMonitor();

	std::function<void()> OnExit;
	std::function<void(int)> OnException;
private:
	bool ShouldRun;

	void WatchThread(int pid);
};
