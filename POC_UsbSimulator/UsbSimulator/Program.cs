using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Castle.Facilities.Logging;
using Castle.Services.Logging.NLogIntegration;
using Castle.Windsor;
using LibUsbDotNet;
using LibUsbDotNet.Descriptors;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using UsbSimulator.RawGadget;
using ILogger = Castle.Core.Logging.ILogger;

namespace UsbSimulator
{
    class Program
    {
        static IWindsorContainer container = new WindsorContainer();

        static void Main(string[] args)
        {
            container.AddFacility<LoggingFacility>(x =>
            {
                x.LogUsing<NLogFactory>()
                    .WithConfig("nlog.config");
            });

            container.Resolve<ILogger>().DebugFormat("Start Application...");

            //UsbDeivce();
            //using (UsbRelayRawGadget gadget = new UsbRelayRawGadget("fe980000.usb", "fe980000.usb")
            //{
            //    Logger = container.Resolve<ILogger>().CreateChildLogger("UsbRawGadget"),
            //})
            //{
            //    UsbContext context = new UsbContext();

            //    gadget.UsbDevice = context.FindAll(x => x.VendorId == 0x046a).First();
            //    gadget.Open().Wait();

            //}
            using (UsbRawGadget gadget = new UsbRawGadget("fe980000.usb", "fe980000.usb")
            {
                Logger = container.Resolve<ILogger>().CreateChildLogger("UsbRawGadget"),
            })
            {
                //UsbContext context = new UsbContext();

                gadget.Open().Wait();

            }

            Console.ReadLine();
        }

        static void UsbDeivce()
        {
            //UsbContext context = new UsbContext();
            //foreach (IUsbDevice item in context.FindAll(x => true))
            //{
            //    item.Open();

            //    Console.WriteLine("Device: {0}", item.Info.Device);
            //    Console.WriteLine("DeviceClass: {0}", item.Info.DeviceClass);
            //    Console.WriteLine("\tVendorId: 0x{0:x}", item.VendorId);
            //    Console.WriteLine("\tProductId: 0x{0:x}", item.ProductId);
            //    Console.WriteLine("\tProduct: {0}", item.Info.Product);
            //    Console.WriteLine("\tManufacturer: {0}", item.Info.Manufacturer);
            //    Console.WriteLine("\tSerialNumber: {0}", item.Info.SerialNumber);
            //}


            //UsbContext context = new UsbContext();
            //IUsbDevice usbDevice = context.FindAll(x => x.VendorId == 0x04f2).First();
            ////IUsbDevice usbDevice = context.Find(new UsbDeviceFinder(0x04f2, 0x3f41));
            //usbDevice.Open();

            //UsbSetupPacket setupPacket = new UsbSetupPacket((byte)EndpointDirection.In | (byte)UsbRequestType.TypeStandard | (byte)UsbRequestRecipient.RecipDevice,
            //    (byte)StandardRequest.GetDescriptor,
            //    BitConverter.ToInt16(new byte[] { 0x00, (byte)DescriptorType.Device }),
            //    0, 18);

            //byte[] buf = new byte[1024];


            //Task.Delay(TimeSpan.FromSeconds(5)).Wait();

            //int uTransferLength = usbDevice.ControlTransfer(setupPacket, buf, 0, buf.Length);

            //Console.WriteLine("uTransferLength: {0}", uTransferLength);
            ////Console.WriteLine("UsbDeviceDescriptor: {0}", t);
        }


        /// <summary>
        /// 将Byte转换为结构体类型
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T ByteToStruct<T>(byte[] bytes)
        {
            int size = Marshal.SizeOf<T>();
            if (size > bytes.Length)
            {
                return default;
            }
            //分配结构体内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷贝到分配好的内存空间
            Marshal.Copy(bytes, 0, structPtr, size);
            //将内存空间转换为目标结构体
            T obj = Marshal.PtrToStructure<T>(structPtr);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            return obj;
        }
    }
}
