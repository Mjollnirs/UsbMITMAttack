using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /* USB_DT_ENDPOINT: Endpoint descriptor */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbEndpointDescriptor
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte bLength;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDescriptorType;

        [MarshalAs(UnmanagedType.U1)]
        public byte bEndpointAddress;

        [MarshalAs(UnmanagedType.U1)]
        public byte bmAttributes;

        [MarshalAs(UnmanagedType.U2)]
        public ushort wMaxPacketSize;

        [MarshalAs(UnmanagedType.U1)]
        public byte bInterval;

        /* NOTE:  these two are _only_ in audio endpoints. */
        /* use USB_DT_ENDPOINT*_SIZE in bLength, not sizeof. */
        [MarshalAs(UnmanagedType.U1)]
        public byte bRefresh;

        [MarshalAs(UnmanagedType.U1)]
        public byte bSynchAddress;
    }
}
