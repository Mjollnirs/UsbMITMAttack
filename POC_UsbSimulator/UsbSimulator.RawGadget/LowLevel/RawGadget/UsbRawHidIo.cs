using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public struct UsbRawHidIo
    {
        public UsbRawEpIo Inner;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8, ArraySubType = UnmanagedType.U1)]
        public byte[] Data;
    }
}
