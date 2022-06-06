using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_eps_info - argument for USB_RAW_IOCTL_EPS_INFO ioctl.
     * eps: Structures that store information about non-control endpoints.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawEpsInfo
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = RawGadgetConst.USB_RAW_EPS_NUM_MAX)]
        public UsbRawEpInfo[] Eps;
    }
}
