using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /* USB_DT_DEVICE: Device descriptor */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbDeviceDescriptor
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

        [MarshalAs(UnmanagedType.U2)]
        public ushort idVendor;

        [MarshalAs(UnmanagedType.U2)]
        public ushort idProduct;

        [MarshalAs(UnmanagedType.U2)]
        public ushort bcdDevice;

        [MarshalAs(UnmanagedType.U1)]
        public byte iManufacturer;

        [MarshalAs(UnmanagedType.U1)]
        public byte iProduct;

        [MarshalAs(UnmanagedType.U1)]
        public byte iSerialNumber;

        [MarshalAs(UnmanagedType.U1)]
        public byte bNumConfigurations;
    }
}
