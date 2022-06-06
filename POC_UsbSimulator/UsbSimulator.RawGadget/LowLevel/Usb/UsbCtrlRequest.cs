using System;
using System.Runtime.InteropServices;
using LibUsbDotNet.Main;

namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    /**
     * struct usb_ctrlrequest - SETUP data for a USB device control request
     * @bRequestType: matches the USB bmRequestType field
     * @bRequest: matches the USB bRequest field
     * @wValue: matches the USB wValue field (le16 byte order)
     * @wIndex: matches the USB wIndex field (le16 byte order)
     * @wLength: matches the USB wLength field (le16 byte order)
     *
     * This structure is used to send control requests to a USB device.  It matches
     * the different fields of the USB 2.0 Spec section 9.3, table 9-2.  See the
     * USB spec for a fuller description of the different fields, and what they are
     * used for.
     *
     * Note that the driver for any interface can issue control requests.
     * For most devices, interfaces don't coordinate with each other, so
     * such requests may be made at any time.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbCtrlRequest
    {
        [MarshalAs(UnmanagedType.U1)]
        public byte bRequestType;
        
        [MarshalAs(UnmanagedType.U1)]
        public byte bRequest;

        [MarshalAs(UnmanagedType.U2)]
        public ushort wValue;

        [MarshalAs(UnmanagedType.U2)]
        public ushort wIndex;

        [MarshalAs(UnmanagedType.U2)]
        public ushort wLength;
    }
}
