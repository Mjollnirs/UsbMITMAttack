using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_ep_io - argument for USB_RAW_IOCTL_EP0/EP_WRITE/READ ioctls.
     * @ep: Endpoint handle as returned by USB_RAW_IOCTL_EP_ENABLE for
     *     USB_RAW_IOCTL_EP_WRITE/READ. Ignored for USB_RAW_IOCTL_EP0_WRITE/READ.
     * @flags: When USB_RAW_IO_FLAGS_ZERO is specified, the zero flag is set on
     *     the submitted USB request, see include/linux/usb/gadget.h for details.
     * @length: Length of data.
     * @data: Data to send for USB_RAW_IOCTL_EP0/EP_WRITE. Buffer to store received
     *     data for USB_RAW_IOCTL_EP0/EP_READ.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawEpIo
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort Ep;

        [MarshalAs(UnmanagedType.U2)]
        public ushort Flags;

        [MarshalAs(UnmanagedType.U4)]
        public uint Length;
    }
}
