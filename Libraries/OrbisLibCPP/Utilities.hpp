#pragma once

bool FileExists(const char* FileName);
BOOL CWriteFile(const char* FilePath, CONST PVOID Data, DWORD Size);
void SetFilePerms(const char* path);