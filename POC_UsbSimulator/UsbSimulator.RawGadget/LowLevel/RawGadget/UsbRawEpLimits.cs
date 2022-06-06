using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_ep_limits - exposes endpoint limits from struct usb_ep.
     * @maxpacket_limit: Maximum packet size value supported by this endpoint.
     * @max_streams: maximum number of streams supported by this endpoint
     *     (actual number is 2^n).
     * @reserved: Empty, reserved for potential future extensions.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawEpLimits
    {
        [MarshalAs(UnmanagedType.U2)]
        public ushort MaxpacketLimit;

        [MarshalAs(UnmanagedType.U2)]
        public ushort MaxStreams;

        [MarshalAs(UnmanagedType.U4)]
        public uint Reserved;
    }
}
