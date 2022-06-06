using System;
using System.Runtime.InteropServices;
using UsbSimulator.RawGadget.LowLevel.Usb;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawControlEvent
    {
        public UsbRawEvent Inner;

        public UsbCtrlRequest Request;
    }
}
