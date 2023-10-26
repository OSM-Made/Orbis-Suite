#pragma once

class Events
{
public:
    enum EventId
    {
        EVENT_EXCEPTION,
        EVENT_CONTINUE,
        EVENT_DIE,
        EVENT_ATTACH,
        EVENT_DETACH,
        EVENT_SUSPEND,
        EVENT_RESUME,
        EVENT_SHUTDOWN,
    };

    static bool Init();
    static void Term();

    static void AddHost(SceNetInAddr_t HostAddress);
    static void RemoveHost(SceNetInAddr_t HostAddress);
    static void SendEvent(int EventId, int pid = -1);

private:
    static std::vector<SceNetInAddr_t> HostList;
    static std::mutex HostListMtx;

    static SceNetId Connect(SceNetInAddr_t HostAddress);
};
