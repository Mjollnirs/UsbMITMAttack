using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /* USB_DT_DEVICE_QUALIFIER: Device Qualifier descriptor */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbQualifierDescriptor
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte bLength;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDescriptorType;

        [MarshalAs(UnmanagedType.U2)]
        public ushort bcdUSB;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDeviceClass;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDeviceSubClass;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDeviceProtocol;

        [MarshalAs(UnmanagedType.U1)]
        public byte bMaxPacketSize0;

        [MarshalAs(UnmanagedType.U1)]
        public byte bNumConfigurations;

        [MarshalAs(UnmanagedType.U1)]
        public byte bRESERVED;
    }
}
