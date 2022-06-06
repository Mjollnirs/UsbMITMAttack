using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawIntIo
    {
        public UsbRawEpIo Inner;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = RawGadgetConst.EP_MAX_PACKET_INT, ArraySubType = UnmanagedType.U1)]
        public byte[] Data;
    }
}
