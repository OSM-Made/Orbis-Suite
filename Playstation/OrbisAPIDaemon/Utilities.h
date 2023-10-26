#pragma once

bool LoadModules();
bool Jailbreak();
bool LoadSymbol(SceKernelModule handle, const char* symbol, void** funcOut);
bool CopySflash();
void SendProtobufPacket(SceNetId sock, const google::protobuf::Message& message);

template<class T>
bool RecieveProtoBuf(SceNetId sock, T* output)
{
	auto rawPacket = Sockets::ReceiveWithSize(sock);

	if (rawPacket.size() <= 0)
	{
		Logger::Error("RecieveProtoBuf(): Failed to recieve the proto packet.\n");
		return false;
	}

	if (!output->ParseFromArray(rawPacket.data(), rawPacket.size()))
	{
		Logger::Error("RecieveProtoBuf(): Failed to parse the proto packet.\n");
		return false;
	}

	return true;
}

void SendStatePacket(SceNetId sock, bool succeeded, const char* fmt, ...);