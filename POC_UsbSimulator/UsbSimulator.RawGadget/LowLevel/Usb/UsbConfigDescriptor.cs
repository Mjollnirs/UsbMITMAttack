using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /* USB_DT_CONFIG: Configuration descriptor information.
     *
     * USB_DT_OTHER_SPEED_CONFIG is the same descriptor, except that the
     * descriptor type is different.  Highspeed-capable devices can look
     * different depending on what speed they're currently running.  Only
     * devices with a USB_DT_DEVICE_QUALIFIER have any OTHER_SPEED_CONFIG
     * descriptors.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbConfigDescriptor
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte bLength;

        [MarshalAs(UnmanagedType.U1)]
        public byte bDescriptorType;

        [MarshalAs(UnmanagedType.U2)]
        public ushort wTotalLength;

        [MarshalAs(UnmanagedType.U1)]
        public byte bNumInterfaces;

        [MarshalAs(UnmanagedType.U1)]
        public byte bConfigurationValue;

        [MarshalAs(UnmanagedType.U1)]
        public byte iConfiguration;

        [MarshalAs(UnmanagedType.U1)]
        public byte bmAttributes;

        [MarshalAs(UnmanagedType.U1)]
        public byte bMaxPower;
    }
}
