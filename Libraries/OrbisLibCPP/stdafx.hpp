#pragma once

#include <stdio.h>
#include <stdlib.h>
#include <iostream>
#include <stdarg.h>
#include <conio.h>
#include <windows.h>
#include <random>
#include <WinSock.h>

#include <tchar.h>
#include <iostream>
#include <string>
//#include <functional>
#include <memory>

using namespace std;

#include <iostream>
#include <Windows.h>
#include <winsock.h>
#include <stdio.h>
#include <stdlib.h>
#include <stdint.h>

#pragma comment (lib, "Ws2_32.lib")

#include "Utilities.hpp"
#include "Sockets.hpp"
#include "SocketListener.hpp"
#include "OrbisDef.hpp"
#include "OrbisLib.hpp"
#include "OrbisProc.hpp"
#include "OrbisTarget.hpp"
#include "OrbisDebugger.hpp"
#include "OrbisService.hpp"
#include "OrbisAPI.hpp"
#include "OrbisTargetManagement.hpp"
#include "OrbisSettings.hpp"

extern bool IsWinService;