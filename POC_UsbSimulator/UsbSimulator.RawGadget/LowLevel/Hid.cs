using System;
namespace UsbSimulator.RawGadget.LowLevel
{
    public static class HidConst
    {
        public const int HID_REQ_GET_REPORT = 0x01;
        public const int HID_REQ_GET_IDLE = 0x02;
        public const int HID_REQ_GET_PROTOCOL = 0x03;
        public const int HID_REQ_SET_REPORT = 0x09;
        public const int HID_REQ_SET_IDLE = 0x0A;
        public const int HID_REQ_SET_PROTOCOL = 0x0B;
    }
}
