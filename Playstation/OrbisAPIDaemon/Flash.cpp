#include "stdafx.h"
#include "Flash.h"

void ReadFlash(off_t Offset, void* Data, unsigned int Size)
{
	int fd = sceKernelOpen("/data/Orbis Suite/sflash0", SCE_KERNEL_O_RDONLY, 0777);
	if (fd)
	{
		sceKernelPread(fd, Data, Size, Offset);
		sceKernelClose(fd);
	}
	else
	{
		Logger::Error("Failed to Open sflash\n");
	}
}