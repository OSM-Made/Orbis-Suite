#pragma once

#define FLASH_FACTORY_FW 0x1C9060 // 4 Bytes
#define FLASH_CURRENT_FW 0x1C9068 // 4 Bytes
#define FLASH_MB_SERIAL 0x1C8000 // 14 Bytes String
#define FLASH_SERIAL 0x1C8030 // 10 Bytes String
#define FLASH_MODEL 0x1C8041 // 14 Bytes String
#define FLASH_LAN_MAC 0x1C4021 // 6 Bytes
#define FLASH_UART_FLAG 0x1C931F // 1 Byte
#define FLASH_IDU_MODE 0x1CA600 // 1 Byte

void ReadFlash(off_t Offset, void* Data, unsigned int Size);