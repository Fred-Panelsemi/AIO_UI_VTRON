using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace AIO
{

    
    class SLUSBXpressDLL
    {
        // Return Codes
        public const Int32 SI_SUCCESS = 0x00;
        public const Int32 SI_DEVICE_NOT_FOUND = 0xFF;
        public const Int32 SI_INVALID_HANDLE = 0x01;
        public const Int32 SI_READ_ERROR = 0x02;
        public const Int32 SI_RX_QUEUE_NOT_READY = 0x03;
        public const Int32 SI_WRITE_ERROR = 0x04;
        public const Int32 SI_RESET_ERROR = 0x05;
        public const Int32 SI_INVALID_PARAMETER = 0x06;
        public const Int32 SI_INVALID_REQUEST_LENGTH = 0x07;
        public const Int32 SI_DEVICE_IO_FAILED = 0x08;
        public const Int32 SI_INVALID_BAUDRATE = 0x09;
        public const Int32 SI_FUNCTION_NOT_SUPPORTED = 0x0a;
        public const Int32 SI_GLOBAL_DATA_ERROR = 0x0b;
        public const Int32 SI_SYSTEM_ERROR_CODE = 0x0c;
        public const Int32 SI_READ_TIMED_OUT = 0x0d;
        public const Int32 SI_WRITE_TIMED_OUT = 0x0e;
        public const Int32 SI_IO_PENDING = 0x0f;

        // GetProductString() function flags
        public const Int32 SI_RETURN_SERIAL_NUMBER = 0x00;
        public const Int32 SI_RETURN_DESCRIPTION = 0x01;
        public const Int32 SI_RETURN_LINK_NAME = 0x02;
        public const Int32 SI_RETURN_VID = 0x03;
        public const Int32 SI_RETURN_PID = 0x04;

        // RX Queue status flags
        public const Int32 SI_RX_NO_OVERRUN = 0x00;
        public const Int32 SI_RX_EMPTY = 0x00;
        public const Int32 SI_RX_OVERRUN = 0x01;
        public const Int32 SI_RX_READY = 0x02;

        // Buffer size limits
        public const Int32 SI_MAX_DEVICE_STRLEN = 256;
        public const Int32 SI_MAX_READ_SIZE = 4096 * 1;
        public const Int32 SI_MAX_WRITE_SIZE = 4096;

        // Input and Output pin Characteristics
        public const Int32 SI_HELD_INACTIVE = 0x00;
        public const Int32 SI_HELD_ACTIVE = 0x01;
        public const Int32 SI_FIRMWARE_CONTROLLED = 0x02;
        public const Int32 SI_RECEIVE_FLOW_CONTROL = 0x02;
        public const Int32 SI_TRANSMIT_ACTIVE_SIGNAL = 0x03;
        public const Int32 SI_STATUS_INPUT = 0x00;
        public const Int32 SI_HANDSHAKE_LINE = 0x01;

        // Mask and Latch value bit definitions
        public const Int32 SI_GPIO_0 = 0x01;
        public const Int32 SI_GPIO_1 = 0x02;
        public const Int32 SI_GPIO_2 = 0x04;
        public const Int32 SI_GPIO_3 = 0x08;

        // GetDeviceVersion() return codes
        public const Int32 SI_CP2101_VERSION = 0x01;
        public const Int32 SI_CP2102_VERSION = 0x02;
        public const Int32 SI_CP2103_VERSION = 0x03;

        public static Int32 Status;
        public static UInt32 hUSBDevice;

        // Flash Key Codes valid for 'F32x/'F34x/'F38x devices.
        public const byte FLASH_KEY0 = 0xA5;   //F380Boot
        public const byte FLASH_KEY1 = 0xF1;   //F380Boot
        //USBXpress Read/Write Timeouts (in ms)
        public const int USBX_READ_TIMEOUT = 500;
        public const int USBX_WRITE_TIMEOUT = 500;

        public const int CMD_GET_DEVICE_INFO = 0x0;
        public const int CMD_SET_PAGE = 0x1;
        public const int CMD_ERASE_PAGE = 0x2;
        public const int CMD_WRITE_PAGE = 0x3;
        public const int CMD_CRC_PAGE = 0x4;
        public const int CMD_SW_RESET = 0x5;
        public const int CMD_SET_FLASH_KEYS = 0x6;
        public const int CMD_WRITE_SIGNATURE = 0x7;
        // The length of the block returned by the BL FW to the BL SW in response to
        // the CMD_GET_DEVICE_INFO command; This excludes the CString in the above
        // structure.
        public const int DEVICE_INFO_BLOCK_RET_LENGTH = 9;
        // CheckRxQueue timeout (in ms)
        //' This timeout is used to expire waiting for data to arrive from device
        //' because the SI_CheckRxQueue function always returns SI_SUCCESS even after the
        //' device has been removed (in case of surprise removal).
        public const int USBX_CHECKRXQUEUE_TIMEOUT = 100;
        //' Location and value of the Validation Signature (16-bit value)
        //' Any changes to these should be reflected in the PC BL Application
        //'#if defined(__C51__)                   // For BL FW only (Keil compiler)
        //'#define APP_FW_VER_ADDR  ((APP_END_PAGE+1) * PAGE_SIZE) - 4
        //'#define SIGNATURE_ADDR  ((APP_END_PAGE+1) * PAGE_SIZE) - 2
        //'#End If
        public const Int32 REF_SIGNATURE = 0x3DC2;         // Reference Validation Signature (for comparison)

        //' Changes to APP_START_PAGE should be mirrored in:
        //' BL FW's STARTUP.A51(INTVEC_TABLE)
        public const int APP_START_PAGE = 0xA;      //'First application page (page 0A)
        public const Int32 PAGE_SIZE = 0x200;      //'Pages are 512 bytes long
        public const Int32 APP_START_ADDR = APP_START_PAGE * PAGE_SIZE;

        //' BL FW <-> SW Protocol Constants
        public const int COMMAND_OK = 0x0;
        public const int COMMAND_FAILED = 0x1;
        public const int UNKNOWN_COMMAND = 0x2;
        public const int SIGNATURE_NOT_ERASED = 0x3;
        public const int FLASH_KEYS_NOT_SET = 0x4;

        public const int APP_END_PAGE_01 = 0x1D;//          ' Last application page  (page 29)
        public const int APP_END_PAGE_02 = 0x1D;//         ' Last application page  (page 29)
        public const int APP_END_PAGE_03 = 0x3E;//          ' Last application page  (page 62)
        public const int APP_END_PAGE_04 = 0x7C;//          ' Last application page  (page 124)
        public const int APP_END_PAGE_05 = 0x3E;//          ' Last application page  (page 62)
        public const int APP_END_PAGE_06 = 0x7C;//          ' Last application page  (page 124)






        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetNumDevices
            (ref Int32 lpdwNumDevices);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetProductString
            (Int32 dwDeviceNum, 
            StringBuilder lpvDeviceString, 
            Int32 dwFlags);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Open
            (Int32 dwDevice, 
            ref UInt32 cyHandle);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Close
            (UInt32 cyHandle);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Read
            (UInt32 cyHandle, 
            ref Byte lpBuffer, 
            Int32 dwBytesToRead, 
            ref Int32 lpdwBytesReturned, 
            Int32 lpOverlapped);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_Write
            (UInt32 cyHandle, 
            ref Byte lpBuffer, 
            Int32 dwBytesToWrite, 
            ref Int32 lpdwBytesWritten, 
            Int32 lpOverlapped);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_SetTimeouts
            (Int32 dwReadTimeout, 
            Int32 dwWriteTimeout);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_GetTimeouts
            (ref Int32 dwReadTimeout, 
            ref Int32 dwWriteTimeout);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_CheckRXQueue
            (UInt32 cyHandle, 
            ref UInt32 lpdwNumBytesInQueue, 
            ref UInt32 lpdwQueueStatus);

        [DllImport("SiUSBXp.dll")]
        public static extern int SI_FlushBuffers
            (UInt32 cyHandle,
            ref Byte TxFlush,
            ref Byte RxFlush);



    }
}
