using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /* USB_DT_STRING: String descriptor */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbStringDescriptor
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte bLength;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDescriptorType;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = UsbConst.USB_MAX_STRING_LEN, ArraySubType = UnmanagedType.U1)]
        public byte[] Data;
    }
}
