#pragma once
#include "SocketListener.h"

class API
{
public:
	static void Init();
	static void Term();

private:
	static std::unique_ptr<SocketListener> Listener;
	const static std::map<int, std::function<void(SceNetId sock)>> APICommands;

	static void ListenerCallback(void* tdParam, SceNetId s, SceNetInAddr sin_addr);
	
};
