#include "stdafx.hpp"
#include "Utilities.hpp"

bool FileExists(const char* FileName)
{
	if (GetFileAttributesA(FileName) == -1) {
		int lastError = GetLastError();
		if (lastError == ERROR_FILE_NOT_FOUND || lastError == ERROR_PATH_NOT_FOUND)
			return false;
	}
	return true;
}

BOOL CWriteFile(const char* FilePath, CONST PVOID Data, DWORD Size)
{
	HANDLE fHandle = CreateFileA(FilePath, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fHandle == INVALID_HANDLE_VALUE) {
		return FALSE;
	}

	DWORD writeSize = Size;
	if (WriteFile(fHandle, Data, writeSize, &writeSize, NULL) != TRUE) {
		return FALSE;
	}
	CloseHandle(fHandle);
	return TRUE;
}

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
	ConvertStringSidToSid(L"S-1-1-0", &psid);

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