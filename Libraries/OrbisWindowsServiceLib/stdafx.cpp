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

string GetFileName(const string& path)
{
	string FileName;

	//remove file path.
	size_t lastpath = path.find_last_of("\\");
	if (lastpath == std::string::npos) return path;
	FileName = path.substr(lastpath + 1, path.size());

	//remove the file extension.
	size_t ext = FileName.find_last_of(".");
	if (ext == std::string::npos) return FileName;
	FileName = FileName.replace(ext, 4, ".log");

	return FileName;
}

void EnableDebugLogs()
{
	//program data path
	char ProgramDataPath[MAX_PATH];
	size_t requiredSize = sizeof(ProgramDataPath);
	getenv_s(&requiredSize, (char*)&ProgramDataPath, requiredSize, "PROGRAMDATA");

	//Get the calling exe name.
	char modulename[MAX_PATH];
	GetModuleFileNameA(NULL, modulename, MAX_PATH);

	//Set our output path.
	char LogPath[MAX_PATH];
	sprintf_s(LogPath, "%s\\Orbis Suite\\Logs\\OrbisService-%s", ProgramDataPath, GetFileName(modulename).c_str());

	char LogFolderPath[MAX_PATH];
	sprintf_s(LogFolderPath, "%s\\Orbis Suite\\Logs", ProgramDataPath, GetFileName(modulename).c_str());

	//Create Log folder if doesnt exist.
	if (!FileIO::DirectoryExists(LogFolderPath))
		FileIO::DirectoryCreate(LogFolderPath);

	//Create Log File.
	if (!FileIO::FileExists(LogFolderPath))
		FileIO::FileCreate(LogFolderPath);

	//Output our prints to a File.
	FILE *stream;
	freopen_s(&stream, LogPath, "a", stdout);
	freopen_s(&stream, LogPath, "a", stderr);
}