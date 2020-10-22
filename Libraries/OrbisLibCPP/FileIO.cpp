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
	if (!WriteFile(fHandle, Data, Size, &writeSize, NULL)) {
		CloseHandle(fHandle);
		return false;
	}

	CloseHandle(fHandle);
	return true;
}

bool FileIO::FileRead(const char* File, char* Data, int64_t Size)
{
	HANDLE fHandle = CreateFile(File, GENERIC_READ, FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fHandle == INVALID_HANDLE_VALUE) {
		return false;
	}

	DWORD dwNumberOfBytesRead = 0;
	if (!ReadFile(fHandle, Data, (int)Size, &dwNumberOfBytesRead, NULL)) {
		CloseHandle(fHandle);
		return false;
	}
	else if (dwNumberOfBytesRead != Size) {
		CloseHandle(fHandle);
		return FALSE;
	}

	CloseHandle(fHandle);
	return true;
}

int64_t FileIO::FileSize(const char* File)
{
	HANDLE fHandle = CreateFile(File, GENERIC_WRITE, FILE_SHARE_WRITE, NULL, OPEN_EXISTING, FILE_ATTRIBUTE_NORMAL, NULL);
	if (fHandle == INVALID_HANDLE_VALUE) {
		return -1;
	}

	LARGE_INTEGER size;
	if (!GetFileSizeEx(fHandle, &size))
	{
		CloseHandle(fHandle);
		return -1;
	}

	CloseHandle(fHandle);
	return size.QuadPart;
}