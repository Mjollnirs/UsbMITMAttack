using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /* USB_DT_INTERFACE: Interface descriptor */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbInterfaceDescriptor
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte bLength;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDescriptorType;

        [MarshalAs(UnmanagedType.U1)]
        public byte bInterfaceNumber;

        [MarshalAs(UnmanagedType.U1)]
        public byte bAlternateSetting;

        [MarshalAs(UnmanagedType.U1)]
        public byte bNumEndpoints;

        [MarshalAs(UnmanagedType.U1)]
        public byte bInterfaceClass;

        [MarshalAs(UnmanagedType.U1)]
        public byte bInterfaceSubClass;

        [MarshalAs(UnmanagedType.U1)]
        public byte bInterfaceProtocol;

        [MarshalAs(UnmanagedType.U1)]
        public byte iInterface;
    }
}
