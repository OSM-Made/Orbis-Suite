#include "stdafx.hpp"
#include "FileIO.hpp"

FileIO::FileIO()
{

}

FileIO::~FileIO()
{

}

bool FileIO::FileExists(const char* File)
{
	if (GetFileAttributes(File) == -1)
	{
		int lastError = GetLastError();
		if (lastError == ERROR_FILE_NOT_FOUND || lastError == ERROR_PATH_NOT_FOUND)
			return false;
	}
	return true;
}

bool FileIO::DirectoryExists(const char* Directory)
{
	if (GetFileAttributes(Directory) == -1)
	{
		int lastError = GetLastError();
		if (lastError == ERROR_FILE_NOT_FOUND || lastError == ERROR_PATH_NOT_FOUND)
			return false;
	}
	return true;
}

bool FileIO::FileCreate(const char* File)
{
	HANDLE fHandle = CreateFile(File, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, CREATE_ALWAYS, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fHandle == INVALID_HANDLE_VALUE) {
		return false;
	}

	CloseHandle(fHandle);
	return true;
}

bool FileIO::DirectoryCreate(const char* Directory)
{
	return CreateDirectory(Directory, NULL);
}

bool FileIO::FileWrite(const char* File, char* Data, int Size)
{
	HANDLE fHandle = CreateFile(File, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fHandle == INVALID_HANDLE_VALUE) {
		return false;
	}

	DWORD writeSize = Size;
	if (WriteFile(fHandle, Data, writeSize, &writeSize, NULL) != true) {
		return false;
	}

	CloseHandle(fHandle);
	return true;
}

bool FileIO::FileRead(const char* File, char* Data, int Size)
{
	HANDLE fHandle = CreateFile(File, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fHandle == INVALID_HANDLE_VALUE) {
		return false;
	}

	DWORD writeSize = Size;
	if (ReadFile(fHandle, Data, writeSize, &writeSize, NULL) != true) {
		return false;
	}

	CloseHandle(fHandle);
	return true;
}