using System;
namespace UsbSimulator.RawGadget.LowLevel.Usb
{
    public enum UsbDeviceSpeed : byte
    {
        USB_SPEED_UNKNOWN = 0,                  /* enumerating */
        USB_SPEED_LOW, USB_SPEED_FULL,          /* usb 1.1 */
        USB_SPEED_HIGH,                         /* usb 2.0 */
        USB_SPEED_WIRELESS,                     /* wireless (usb 2.5) */
        USB_SPEED_SUPER,                        /* usb 3.0 */
        USB_SPEED_SUPER_PLUS,                   /* usb 3.1 */
    }
}
