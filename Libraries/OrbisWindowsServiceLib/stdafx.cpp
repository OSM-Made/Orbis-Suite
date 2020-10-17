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