#include "stdafx.h"
#include "Events.h"

std::vector<SceNetInAddr_t> Events::HostList;
std::mutex Events::HostListMtx;

void Events::AddHost(SceNetInAddr_t HostAddress)
{
	// Aquire a lock for the list.
	std::unique_lock<std::mutex> lock(HostListMtx);

	// Add the host to the list if it does not exist already.
	if (std::find(HostList.begin(), HostList.end(), HostAddress) == HostList.end())
	{
		Logger::Info("New host (%i.%i.%i.%i)\n", HostAddress & 0xFF, (HostAddress >> 8) & 0xFF, (HostAddress >> 16) & 0xFF, (HostAddress >> 24) & 0xFF);
		HostList.push_back(HostAddress);
	}
}

void Events::RemoveHost(SceNetInAddr_t HostAddress)
{
	// Aquire a lock for the list.
	std::unique_lock<std::mutex> lock(HostListMtx);

	// Remove this host if it exists in the list.
	if (std::find(HostList.begin(), HostList.end(), HostAddress) != HostList.end())
	{
		Logger::Info("Lost host (%i.%i.%i.%i)\n", HostAddress & 0xFF, (HostAddress >> 8) & 0xFF, (HostAddress >> 16) & 0xFF, (HostAddress >> 24) & 0xFF);
		std::remove(HostList.begin(), HostList.end(), HostAddress);
	}
}

void Events::SendEvent(int EventId, int pid)
{
	if (HostList.empty())
	{
		Logger::Error("SendEvent(): Host List Empty :(\n");
		return;
	}

	for (const auto& host : HostList)
	{
		// Aquire a lock for the list.
		std::unique_lock<std::mutex> lock(HostListMtx);

		Logger::Info("SendEvent(%d): Sending for host %i.%i.%i.%i\n", EventId, host & 0xFF, (host >> 8) & 0xFF, (host >> 16) & 0xFF, (host >> 24) & 0xFF);

		auto sock = Sockets::Connect(host, EVENT_PORT, 4);
		if (sock)
		{
			// Send EventId
			Sockets::SendInt(sock, EventId);

			if (EventId == EVENT_ATTACH && pid != -1)
			{
				Sockets::SendInt(sock, pid);
			}

			// Close the socket.
			sceNetSocketClose(sock);
		}
		else
		{
			Logger::Error("SendEvent(): Failed to connect to host %i.%i.%i.%i\n", host & 0xFF, (host >> 8) & 0xFF, (host >> 16) & 0xFF, (host >> 24) & 0xFF);

			RemoveHost(host);
		}
	}
}