using System;
using System.Runtime.InteropServices;
using UsbSimulator.RawGadget.LowLevel.Usb;

namespace UsbSimulator.RawGadget.LowLevel.RawGadget
{
    /*
     * struct usb_raw_init - argument for USB_RAW_IOCTL_INIT ioctl.
     * @speed: The speed of the emulated USB device, takes the same values as
     *     the usb_device_speed enum: USB_SPEED_FULL, USB_SPEED_HIGH, etc.
     * @driver_name: The name of the UDC driver.
     * @device_name: The name of a UDC instance.
     *
     * The last two fields identify a UDC the gadget driver should bind to.
     * For example, Dummy UDC has "dummy_udc" as its driver_name and "dummy_udc.N"
     * as its device_name, where N in the index of the Dummy UDC instance.
     * At the same time the dwc2 driver that is used on Raspberry Pi Zero, has
     * "20980000.usb" as both driver_name and device_name.
     */
    [StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Unicode)]
    public unsafe struct UsbRawInit
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = RawGadgetConst.UDC_NAME_LENGTH_MAX, ArraySubType = UnmanagedType.U1)]
        public byte[] DriverName;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = RawGadgetConst.UDC_NAME_LENGTH_MAX, ArraySubType = UnmanagedType.U1)]
        public byte[] DeviceName;

        [MarshalAs(UnmanagedType.U1)]
        public UsbDeviceSpeed Speed;
    }
}
