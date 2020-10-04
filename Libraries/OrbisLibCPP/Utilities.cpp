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