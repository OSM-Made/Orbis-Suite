#include "stdafx.h"
#include "SocketListener.h"
#include "API.h"
#include "Target.h"
#include "Debug.h"
#include "Library.h"
#include "Apps.h"

std::unique_ptr<SocketListener> API::Listener;

// Register Command call backs.
const std::map<int, std::function<void(SceNetId s)>> API::APICommands =
{
	// Apps Commands
	{ API_APPS_CHECK_VER, Apps::CheckVersion },
	{ API_APPS_GET_DB, Apps::GetDB },
	{ API_APPS_GET_INFO_STR, Apps::GetAppInfoString },
	{ API_APPS_STATUS, Apps::SendAppStatus },
	{ API_APPS_START, Apps::StartApp },
	{ API_APPS_STOP, Apps::KillApp },
	{ API_APPS_SUSPEND, Apps::SuspendApp },
	{ API_APPS_RESUME, Apps::ResumeApp },
	{ API_APPS_DELETE, Apps::DeleteApp },
	{ API_APPS_SET_VISIBILITY, Apps::SetVisibility },
	{ API_APPS_GET_VISIBILITY, Apps::GetVisibility },

	// Debug Commands
	{ API_DBG_ATTACH, Debug::Attach },
	{ API_DBG_DETACH, Debug::Detach },
	{ API_DBG_GET_CURRENT, Debug::Current },
	{ API_DBG_READ, [](SceNetId s) { Debug::RWMemory(s, false); } },
	{ API_DBG_WRITE, [](SceNetId s) { Debug::RWMemory(s, true); } },
	//API_DBG_KILL,
	//API_DBG_BREAK,
	//API_DBG_RESUME,
	//API_DBG_SIGNAL,
	//API_DBG_STEP,
	//API_DBG_STEP_OVER,
	//API_DBG_STEP_OUT,
	//API_DBG_GET_CALLSTACK,
	//API_DBG_GET_REG,
	//API_DBG_SET_REG,
	//API_DBG_GET_FREG,
	//API_DBG_SET_FREG,
	//API_DBG_GET_DBGREG,
	//API_DBG_SET_DBGREG,

	// Library Commands
	{ API_DBG_LOAD_LIBRARY, Library::LoadLibrary },
	{ API_DBG_UNLOAD_LIBRARY, Library::UnloadLibrary },
	{ API_DBG_RELOAD_LIBRARY, Library::ReloadLibrary },
	{ API_DBG_LIBRARY_LIST, Library::GetLibraryList },

	// Target Commands
	{ API_TARGET_INFO, Target::SendTargetInfo },
	{ API_TARGET_RESTMODE, [](SceNetId) { ChangeSystemState(NewSystemState::Suspend); } },
	{ API_TARGET_SHUTDOWN, [](SceNetId) { ChangeSystemState(NewSystemState::Shutdown); } },
	{ API_TARGET_REBOOT, [](SceNetId) { ChangeSystemState(NewSystemState::Reboot); } },
	{ API_TARGET_NOTIFY, Target::DoNotify },
	{ API_TARGET_BUZZER, Target::DoBuzzer },
	{ API_TARGET_SET_LED, Target::SetConsoleLED },
	{ API_TARGET_SET_SETTINGS, Target::SetSettings },
	{ API_TARGET_GET_PROC_LIST, Target::ProcList },
	{ API_TARGET_SEND_FILE, Target::SendFile },
	{ API_TARGET_RECIEVE_FILE, Target::RecieveFile },
	{ API_TARGET_DELETE_FILE, Target::DeleteFile },
};

void API::ListenerCallback(void* tdParam, SceNetId s, SceNetInAddr sin_addr)
{
	// Recieve the first 4 bytes.
	int magicNumber;
	if (!Sockets::RecvInt(s, &magicNumber))
	{
		// Spams but honestly if this fails all is lost.
		// Logger::Error("[API] Failed to recieve the magic number.\n");
		return;
	}

	// Api Check magic.
	if (magicNumber == 0xFEED)
		return;

	// Check the magic number so we dont get bogus packets.
	if (magicNumber != 0xDEADBEEF)
	{
		Sockets::SendInt(s, 0);
		Logger::Error("[API] Magic number miss match! We got %llX but we expected %llX.\n", magicNumber, 0xDEADBEEF);
		return;
	}

	// Reply back with the accepted value.
	Sockets::SendInt(s, 1);

	// Get the initial packet.
	InitialPacket packet;
	if (!RecieveProtoBuf<InitialPacket>(s, &packet))
	{
		Logger::Error("[API] Failed to recieve the initial proto packet.\n");
		return;
	}

	// Validate Packet
	if (packet.packetversion() != PACKET_VERSION)
	{
		SendStatePacket(s, false, "Packet version miss match expected packet version %d.", PACKET_VERSION);
		Logger::Error("[API] Outdated packet version recieved %d but expected version %d.\n", packet.packetversion(), PACKET_VERSION);
		return;
	}

	// Add host to the host list.
	Events::AddHost(sin_addr.s_addr);

	// Find the command in the map
	auto it = API::APICommands.find(packet.command());

	// Check if the command exists in the map
	if (it != API::APICommands.end())
	{
		// Let the host know we have this command and are ready to move forward.
		SendStatePacket(s, true, "");

		// Call the command function with the given argument
		it->second(s);
	}
	else
	{
		SendStatePacket(s, false, "Command %d is not implemented at this time.", packet.command());
		Logger::Error("[API] Command %d is not implemented at this time.\n", packet.command());
	}
}

void API::Init()
{
	Listener = std::make_unique<SocketListener>(ListenerCallback, nullptr, API_PORT);
}

void API::Term()
{

}