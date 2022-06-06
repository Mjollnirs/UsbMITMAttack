using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_ep_info - stores information about a gadget endpoint.
     * @name: Name of the endpoint as it is defined in the UDC driver.
     * @addr: Address of the endpoint that must be specified in the endpoint
     *     descriptor passed to USB_RAW_IOCTL_EP_ENABLE ioctl.
     * @caps: Endpoint capabilities.
     * @limits: Endpoint limits.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawEpInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = RawGadgetConst.USB_RAW_EP_NAME_MAX, ArraySubType = UnmanagedType.U1)]
        public byte[] Name;

        [MarshalAs(UnmanagedType.U4)]
        public uint Address;

        public UsbRawEpCaps Caps;

        public UsbRawEpLimits Limits;
    }
}
