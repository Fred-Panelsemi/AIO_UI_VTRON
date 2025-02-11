using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIO
{
    //
    // 摘要:
    //     Mapping area of the receiving card on the LED display
    public struct ScanBoardMapRegion// : ICloneable
    {
        //
        // 摘要:
        //     Multi Screen
        public byte IPindex;
        //
        // 摘要:
        //     Sending card (or controller) index
        public byte SenderIndex;
        //
        // 摘要:
        //     Index of the Ethernet port connected to the sending card (or controller)
        public byte PortIndex;
        //
        // 摘要:
        //     Sequence number of the receiving card connected to the Ethernet port
        public ushort ConnectIndex;
        //
        // 摘要:
        //     X offset of the receiving card on the LED display
        public ushort X;
        //
        // 摘要:
        //     Y offset of the receiving card on the LED display
        public ushort Y;
        //
        // 摘要:
        //     Horizontal resolution of the loading capacity of the receiving card
        public ushort Width;
        //
        // 摘要:
        //     Vertical resolution of the loading capacity of the receiving card
        public ushort Height;
        //
        // 摘要:
        //     Clone
        //public object Clone();
    }

    public struct PrimaryMapRegion// : ICloneable
    {
        //
        // 摘要:
        //     Multi Screen
        public byte Rec;
        //
        // 摘要:
        //     Sending card (or controller) index
        public byte Sen;
        //
        // 摘要:
        //     Index of the Ethernet port connected to the sending card (or controller)
        public byte Po;
        //
        // 摘要:
        //     Sequence number of the receiving card connected to the Ethernet port
        public byte Ca;
        //
        // 摘要:
        //     X offset of the receiving card on the LED display
        public int X;
        //
        // 摘要:
        //     Y offset of the receiving card on the LED display
        public int Y;
        //
        // 摘要:
        //     Horizontal resolution of the loading capacity of the receiving card
        public int W;
        //
        // 摘要:
        //     Vertical resolution of the loading capacity of the receiving card
        public int H;
        //
        // 摘要:
        //     Clone
        //public object Clone();
        //
        // 摘要:
        //     Vertical resolution of the loading capacity of the receiving card
        public int Gap;
    }

    public class LEDScreenInfo //: ICloneable
    {
        //
        // 摘要:
        //     Resolution W of the screen
        public ushort ResW;
        //
        // 摘要:
        //     Resolution H of the screen
        public ushort ResH;
        //
        // 摘要:
        //     Horizontal position of the screen
        public ushort ScreenX;
        //
        // 摘要:
        //     Vertical position of the screen
        public ushort ScreenY;
        //
        // 摘要:
        //     Virtual mode of a screen
        public ScreenVirtualMode VirtualMode;
        //
        // 摘要:
        //     Receiving card list of the screen
        public List<ScanBoardMapRegion> ScanBoardInfoList;
        public List<PrimaryMapRegion> primaryInfoList;
        //
        // 摘要:
        //     Screen type
        public LEDDisplyType LEDDisplyType { get; set; }

        //
        // 摘要:
        //     Clone screen information
        //public object Clone();
        //
        // 摘要:
        //     Copy screen information
        //
        // 參數:
        //   obj:
        //
        // 傳回:
        //     True: Successful; False: Failed
        //public bool CopyTo(object obj);
    }

    public enum ScreenVirtualMode
    {
        //
        // 摘要:
        //     Not virtual
        Disable = 0,
        //
        // 摘要:
        //     4-LED virtual mode 1
        Led4Mode1 = 1,
        //
        // 摘要:
        //     4-LED virtual mode 2
        Led4Mode2 = 2,
        //
        // 摘要:
        //     3-LED virtual
        Led3Mode = 3
    }

    public enum LEDDisplyType
    {
        SimpleSingleType = 0,
        StandardType = 1,
        ComplexType = 2
    }


    

}
