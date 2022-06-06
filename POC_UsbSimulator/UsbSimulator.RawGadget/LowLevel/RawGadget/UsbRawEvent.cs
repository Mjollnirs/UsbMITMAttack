using System;
using System.Runtime.InteropServices;
using UsbSimulator.RawGadget.LowLevel.Usb;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_event - argument for USB_RAW_IOCTL_EVENT_FETCH ioctl.
     * @type: The type of the fetched event.
     * @length: Length of the data buffer. Updated by the driver and set to the
     *     actual length of the fetched event data.
     * @data: A buffer to store the fetched event data.
     *
     * Currently the fetched data buffer is empty for USB_RAW_EVENT_CONNECT,
     * and contains struct usb_ctrlrequest for USB_RAW_EVENT_CONTROL.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawEvent
    {
        [MarshalAs(UnmanagedType.U4)]
        public UsbRawEventType EventType;

        [MarshalAs(UnmanagedType.U4)]
        public uint Length;
    }

    /* The type of event fetched with the USB_RAW_IOCTL_EVENT_FETCH ioctl. */
    public enum UsbRawEventType : uint
    {
        USB_RAW_EVENT_INVALID = 0,

        /* This event is queued when the driver has bound to a UDC. */
        USB_RAW_EVENT_CONNECT = 1,

        /* This event is queued when a new control request arrived to ep0. */
        USB_RAW_EVENT_CONTROL = 2,

        /* The list might grow in the future. */
    }
}
