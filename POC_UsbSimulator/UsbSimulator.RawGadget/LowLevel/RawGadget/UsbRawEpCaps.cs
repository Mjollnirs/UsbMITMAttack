using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_ep_caps - exposes endpoint capabilities from struct usb_ep
     *     (technically from its member struct usb_ep_caps).
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawEpCaps
    {
        public UsbRawEpCapsEnum Enum;
    }

    [Flags]
    public enum UsbRawEpCapsEnum
    {
        None = 0,
        TypeControl = 1 << 0,
        TypeIso = 1 << 1,
        TypeBulk = 1 << 2,
        TypeInt = 1 << 3,
        DirIn = 1 << 4,
        DirOut = 1 << 5,
        All = TypeControl | TypeIso | TypeBulk | TypeInt | DirIn | DirOut,
    }
}
