#pragma once

class FileIO
{
public:
	FileIO();
	~FileIO();

	static bool FileExists(const char* File);
	static bool DirectoryExists(const char* Directory);
	static bool FileCreate(const char* File);
	static bool DirectoryCreate(const char* Directory);
	static bool FileWrite(const char* File, char* Data, int Size);
	static bool FileRead(const char* File, char* Data, int64_t Size);
	static int64_t FileSize(const char* File);
private:

};

