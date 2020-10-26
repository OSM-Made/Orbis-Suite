#include "stdafx.hpp"
#include "Utilities.hpp"

#pragma comment(lib, "Advapi32.lib") 

#include "Aclapi.h"
#include "Sddl.h"
#include <io.h>
#include <sys/stat.h>

void SetFilePerms(const char* path)
{
	PACL pDacl, pNewDACL;
	EXPLICIT_ACCESS ExplicitAccess;
	PSECURITY_DESCRIPTOR ppSecurityDescriptor;
	PSID psid;

	GetNamedSecurityInfoA((char*)path, SE_FILE_OBJECT, DACL_SECURITY_INFORMATION, NULL, NULL, &pDacl, NULL, &ppSecurityDescriptor);
	ConvertStringSidToSid("S-1-1-0", &psid);

	ExplicitAccess.grfAccessMode = SET_ACCESS;
	ExplicitAccess.grfAccessPermissions = GENERIC_ALL;
	ExplicitAccess.grfInheritance = CONTAINER_INHERIT_ACE | OBJECT_INHERIT_ACE;
	ExplicitAccess.Trustee.MultipleTrusteeOperation = NO_MULTIPLE_TRUSTEE;
	ExplicitAccess.Trustee.pMultipleTrustee = NULL;
	ExplicitAccess.Trustee.ptstrName = (LPTSTR)psid;
	ExplicitAccess.Trustee.TrusteeForm = TRUSTEE_IS_SID;
	ExplicitAccess.Trustee.TrusteeType = TRUSTEE_IS_UNKNOWN;

	SetEntriesInAcl(1, &ExplicitAccess, pDacl, &pNewDACL);
	SetNamedSecurityInfoA((char*)path, SE_FILE_OBJECT, DACL_SECURITY_INFORMATION, NULL, NULL, pNewDACL, NULL);

	LocalFree(pNewDACL);
	LocalFree(psid);
}

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
	sprintf_s(LogPath, "%s\\Orbis Suite\\Logs\\OrbisLib-%s", ProgramDataPath, GetFileName(modulename).c_str());

	char LogFolderPath[MAX_PATH];
	sprintf_s(LogFolderPath, "%s\\Orbis Suite\\Logs", ProgramDataPath, GetFileName(modulename).c_str());

	//Create Log folder if doesnt exist.
	if (!FileIO::DirectoryExists(LogFolderPath))
		FileIO::DirectoryCreate(LogFolderPath);

	//Create Log File.
	if (!FileIO::FileExists(LogPath))
		FileIO::FileCreate(LogPath);

	//Output our prints to a File.
	FILE *stream;
	freopen_s(&stream, LogPath, "a", stdout);
	freopen_s(&stream, LogPath, "a", stderr);
}