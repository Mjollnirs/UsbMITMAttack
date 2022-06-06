using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawControlIo
    {
        public UsbRawEpIo Inner;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = RawGadgetConst.EP0_MAX_DATA, ArraySubType = UnmanagedType.U1)]
        public byte[] Data;
    }
}
