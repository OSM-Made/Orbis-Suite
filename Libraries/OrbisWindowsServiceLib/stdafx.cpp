#include "stdafx.h"

ServiceClient* Client = NULL;
SocketListener* TargetListener = NULL;
ServiceTargetWatcher* TargetWatcher = NULL;
bool ServiceRunning = false;

const char* TargetCommandsStr[] =
{
	"CMD_PRINT",

	"CMD_INTERCEPT",
	"CMD_CONTINUE",

	"CMD_PROC_DIE",
	"CMD_PROC_ATTACH",
	"CMD_PROC_DETACH",

	"CMD_TARGET_SUSPEND",
	"CMD_TARGET_RESUME",
	"CMD_TARGET_SHUTDOWN",
	"CMD_TARGET_NEWTITLE",

	"CMD_DB_TOUCHED"

	"CMD_TARGET_AVAILABILITY",
};

void socketprint(const char* fmt, ...)
{
	char buffer[0x400] = { };
	va_list args;
	va_start(args, fmt);
	vsprintf_s(buffer, fmt, args);

	Sockets* Sock = new Sockets("127.0.0.1", 9902);

	if (!Sock->Connect())
		goto exit;

	Sock->Send(buffer, sizeof(buffer));

	Sock->Close();

	exit:
	va_end(args);
	delete Sock;
}