#pragma once

#include <stdlib.h>
#include <mutex>
#include <vector>
#include <queue>
#include <functional>
#include <thread> 
#include <kernel.h>
#include <net.h>
#include <user_service.h>
#include <system_service.h>
#include <kernel.h>
#include <map>
#include <memory>

// Custom linked dependancies.
#include <KernelExt.h>
#include <SysmoduleInternal.h>
#include <AppInstUtil.h>
#include <SysCoreUtil.h>
#include <LncUtil.h>

// Custom Utility Library
#include <StringUtils.h>
#include <ThreadPool.h>
#include <Logging.h>
#include <Logger.h>
#include <Process.h>
#include <Utilities.h>
#include <Sockets.h>
#include <FileSystem.h>

using namespace OrbisUtils;

#include "APIPackets.pb.h"
#include "Version.h"
#include "Config.h"
#include "Utilities.h"
#include "Events.h"