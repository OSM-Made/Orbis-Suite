#include "stdafx.h"
#include "Library.h"
#include "Debug.h"
#include <FusionDriver.h>
#include <ShellCode.h>

void Library::LoadLibrary(SceNetId s)
{
	if (!Debug::CheckDebug(s))
		return;

	SPRXPacket packet;
	RecieveProtoBuf(s, &packet);

	// Load the library.
	auto handle = Fusion::LoadSprx(Debug::CurrentPID, packet.path().c_str());

	// Once I can migrate from hen I can error handle here better.
	if (handle <= 0)
	{
		SendStatePacket(s, false, "Failed to load the SPRX %s.", packet.path().c_str());
		return;
	}
	else
		SendStatePacket(s, true, "");

	// Send the result.
	Sockets::SendInt(s, handle);
}

void Library::UnloadLibrary(SceNetId s)
{
	if (!Debug::CheckDebug(s))
		return;

	SPRXPacket packet;
	RecieveProtoBuf(s, &packet);

	// Unload the library.
	auto result = Fusion::UnloadSprx(Debug::CurrentPID, packet.handle());

	// Once I can migrate from hen I can error handle here better.
	if (result != 0)
	{
		SendStatePacket(s, false, "Failed to unload the SPRX %s.", packet.path().c_str());
		return;
	}
	else
		SendStatePacket(s, true, "");
}

void Library::ReloadLibrary(SceNetId s)
{
	if (!Debug::CheckDebug(s))
		return;

	SPRXPacket packet;
	RecieveProtoBuf(s, &packet);

	// Get Process name.
	char processName[32];
	sceKernelGetProcessName(Debug::CurrentPID, processName);

	// Unload the library.
	auto result = Fusion::UnloadSprx(Debug::CurrentPID, packet.handle());
	if (result != 0)
	{
		Logger::Error("Failed to unload %d\n", packet.handle());

		Sockets::SendInt(s, result);

		return;
	}

	// Load the library.
	auto handle = Fusion::LoadSprx(Debug::CurrentPID, packet.path().c_str());

	// Once I can migrate from hen I can error handle here better.
	if (handle <= 0)
	{
		SendStatePacket(s, false, "Failed to reload the SPRX %s.", packet.path().c_str());
		return;
	}
	else
	{
		// Send the results.
		SendStatePacket(s, true, "");
		Sockets::SendInt(s, handle);
	}
}

void Library::GetLibraryList(SceNetId s)
{
	LibraryListPacket packet;

	if (!Debug::CheckDebug(s))
		return;

	auto libraries = std::make_unique<OrbisLibraryInfo[]>(256);
	int actualCount = Fusion::GetLibraryList(Debug::CurrentPID, libraries.get(), 256);

	// Populate the vector list.
	std::vector<LibraryInfoPacket> vectorList;
	for (int i = 0; i < actualCount; i++)
	{
		LibraryInfoPacket infoPacket;
		infoPacket.set_handle(libraries[i].Handle);
		infoPacket.set_path(libraries[i].Path);
		infoPacket.set_mapbase(libraries[i].MapBase);
		infoPacket.set_mapsize(libraries[i].MapSize);
		infoPacket.set_textsize(libraries[i].TextSize);
		infoPacket.set_database(libraries[i].DataBase);
		infoPacket.set_datasize(libraries[i].DataSize);
		vectorList.push_back(infoPacket);
	}

	// Set the parsed list into the protobuf packet.
	*packet.mutable_libraries() = { vectorList.begin(), vectorList.end() };

	// Send the list to host.
	SendProtobufPacket(s, packet);
}