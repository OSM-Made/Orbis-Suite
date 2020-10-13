// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#define WIN32_LEAN_AND_MEAN             // Exclude rarely-used stuff from Windows headers

// reference additional headers your program requires here

#include <iostream>
#include <Windows.h>
#include <winsock.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

#pragma comment (lib, "Ws2_32.lib")

#include "sockets.h"
#include "SocketListener.h"
#include "ServiceClient.h"
#include "ServiceTargetWatcher.h"

#include "../OrbisLibCPP/OrbisDef.hpp"
#include "../OrbisLibCPP/OrbisLib.hpp"
#include "../OrbisLibCPP/OrbisTarget.hpp"
#include "../OrbisLibCPP/OrbisSettings.hpp"
#include "../OrbisLibCPP/OrbisTargetManagement.hpp"

extern "C" _declspec(dllimport) void SetupCPP(bool WinService);
extern "C" __declspec(dllimport) void DestroyCPP();
extern __declspec(dllimport) OrbisLib* orbisLib;

#define PORT_COMMANDSERVER 6902
#define PORT_TARGETSERVER 6901
#define MAX_TARGETS 20

extern ServiceClient* Client;
extern SocketListener* TargetListener;
extern ServiceTargetWatcher* TargetWatcher;
extern bool ServiceRunning;

extern const char* TargetCommandsStr[];