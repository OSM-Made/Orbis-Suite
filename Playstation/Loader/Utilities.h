#pragma once

#define ARRAY_COUNT(arry) sizeof(arry) / sizeof(arry[0])

bool LoadModules();
bool Jailbreak();

void InstallDaemon(const char* Daemon, const char* libs[], int libCount);
void InstallOrbisToolbox();
void InstallOrbisSuite();