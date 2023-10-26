#pragma once

class Target
{
public:
	static void SendTargetInfo(SceNetId sock);
	static void DoNotify(SceNetId sock);
	static void DoBuzzer(SceNetId sock);
	static void SetConsoleLED(SceNetId sock);
	static void SetSettings(SceNetId Sock);
	static void ProcList(SceNetId sock);
	static void SendFile(SceNetId Sock);
	static void RecieveFile(SceNetId sock);
	static void DeleteFile(SceNetId sock);

private:

};
