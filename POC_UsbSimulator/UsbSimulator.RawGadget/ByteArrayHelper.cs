using System;
namespace UsbSimulator.RawGadget
{
    public static class ByteArrayHelper
    {
        public static byte[] PadRight(this byte[] array, int length)
        {
            byte[] data = new byte[length];

            array.CopyTo(data, 0);

            return data;
        }
    }
}
