// stdafx.h : include file for standard system include files,
// or project specific include files that are used frequently, but
// are changed infrequently
//

#pragma once

#include "targetver.h"

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
#include "ServiceLib.h"
