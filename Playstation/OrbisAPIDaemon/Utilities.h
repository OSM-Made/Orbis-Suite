#pragma once

bool LoadModules();
bool LoadSymbol(SceKernelModule handle, const char* symbol, void** funcOut);
bool CopySflash();

template<class T>
bool SendProtobufPacket(SceNetId sock, T* message)
{
	// Make room for the data.
	std::vector<uint8_t> data;
	data.resize(message->ByteSizeLong());

	// Serialize the data.
	if (!message->SerializeToArray(data.data(), data.size()))
	{
		Logger::Error("Failed to serialize the protobuf message.");
		return false;
	}

	// Send the Protobuf packet.
	if (!Sockets::SendWithSize(sock, data.data(), data.size()))
	{
		Logger::Error("Failed to send the serialized protobuf packet.");
		return false;
	}

	return true;
}

template<class T>
bool RecieveProtoBuf(SceNetId sock, T* output)
{
	auto rawPacket = Sockets::ReceiveWithSize(sock);

	if (rawPacket.size() <= 0)
	{
		Logger::Error("Failed to recieve the proto packet.");
		return false;
	}

	if (!output->ParseFromArray(rawPacket.data(), rawPacket.size()))
	{
		Logger::Error("Failed to parse the proto packet.");
		return false;
	}

	return true;
}

void SendStatePacket(SceNetId sock, bool succeeded, const char* fmt, ...);