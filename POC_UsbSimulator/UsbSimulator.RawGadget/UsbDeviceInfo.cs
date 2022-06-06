using System;
namespace UsbSimulator.RawGadget
{
public class UsbDeviceInfo
{
    public ushort Vendor { get; set; } = 0x4fe;

    public string Manufacturer { get; set; } = "PFU Limited";

    public ushort Product { get; set; } = 0x21;

    public string ProductName { get; set; } = "HHKB-Hybrid";

    public string SerialNumber { get; set; } = "UsbSimulator SerialNumber";

    public int DeviceClass { get; set; } = 0x00;

    public int DeviceSubClass { get; set; } = 0x00;

    public int DeviceProtocol { get; set; } = 0x00;

    public ushort Version { get; set; } = 0x001;
}
}
