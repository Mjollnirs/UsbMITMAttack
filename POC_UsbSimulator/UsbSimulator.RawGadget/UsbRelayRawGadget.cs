using System;
using System.Linq;
using System.Threading.Tasks;
using LibUsbDotNet;
using LibUsbDotNet.LibUsb;
using LibUsbDotNet.Main;
using UsbSimulator.RawGadget.LowLevel;
using UsbSimulator.RawGadget.LowLevel.RawGadget;
using UsbSimulator.RawGadget.LowLevel.Usb;

namespace UsbSimulator.RawGadget
{
    public class UsbRelayRawGadget : UsbRawGadget
    {
        public IUsbDevice UsbDevice;

        public UsbRelayRawGadget()
            : base()
        {
        }

        public UsbRelayRawGadget(string driver, string device)
            : base("/dev/raw-gadget", driver, device)
        {
        }

        public UsbRelayRawGadget(string path, string driver, string device)
            : base(path, driver, device)
        {
        }

        protected override async Task Init()
        {
            await base.Init();

            if (UsbDevice != null)
            {
                Logger.DebugFormat("Open Usb Device: {0},{1}", UsbDevice.Info.Manufacturer, UsbDevice.Info.Product);
                UsbDevice.Open();
            }
        }

        private int int_id = -1;
        private int maxPackage = -1;
        protected override async Task EpLoop()
        {
            Logger.DebugFormat("EpLoop.....");

            await Task.Yield();



            while (true)
            {
                var reader = UsbDevice.OpenEndpointReader(ReadEndpointID.Ep01);

                byte[] data = new byte[8];
                int tr;
                var error = reader.Read(data, 0, 8, 300, out tr);

                if (tr != 0)
                {
                    UsbRawHidIo io = new UsbRawHidIo()
                    {
                        Inner = new UsbRawEpIo()
                        {
                            Ep = (ushort)int_id,
                            Flags = 0,
                            Length = (uint)data.Length,
                        },
                        Data = data.PadRight(8),
                    };

                    Logger.InfoFormat("EP: {{1}}, Data: {1}", 1, byteToHexStr(data));


                    EpWrite<UsbRawHidIo>(io).Wait();
                }
            }
        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    returnStr += " ";
                    returnStr += bytes[i].ToString("x2");
                }
            }
            return returnStr;
        }


        protected override async Task<(bool, bool, UsbRawControlIo)> Ep0Request(UsbRawControlEvent @event, bool done)
        {
            await Task.Yield();

            UsbRawControlIo io = new UsbRawControlIo();

            Logger.DebugFormat("Ep0Request RequestType: {0}", @event.Request.bRequestType);

            switch (@event.Request.bRequestType & UsbConst.USB_TYPE_MASK)
            {
                case UsbConst.USB_TYPE_STANDARD:
                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD, REQ: {0}", @event.Request.bRequest);

                    switch (@event.Request.bRequest)
                    {
                        case UsbConst.USB_REQ_SET_CONFIGURATION:
                            Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_SET_CONFIGURATION");

                            var configs = UsbDevice.Configs.First();
                            Logger.DebugFormat("Configs {0}", configs.Configuration);

                            foreach (var @interfaces in configs.Interfaces)
                            {
                                foreach (var _item in @interfaces.Endpoints)
                                {
                                    Logger.DebugFormat("Enable EP: {0}", _item.EndpointAddress);

                                    int_id = EnableEp(new UsbEndpointDescriptor()
                                    {
                                        bLength = UsbConst.USB_DT_ENDPOINT_SIZE,
                                        bDescriptorType = UsbConst.USB_DT_ENDPOINT,
                                        bEndpointAddress = _item.EndpointAddress,
                                        bmAttributes = _item.Attributes,
                                        wMaxPacketSize = _item.MaxPacketSize,
                                        bInterval = _item.Interval,
                                        bSynchAddress = _item.SyncAddress,
                                    }).Result;
                                    maxPackage = _item.MaxPacketSize;

                                    Logger.DebugFormat("Enable EP Success: {0}", int_id);
                                    break;
                                }
                            }

                            Logger.DebugFormat("Enable end...");

                            await VbusDraw(50);
                            await Configure();

                            io.Inner.Length = 0;
                            break;
                        default:
                            io = await Reply(@event, io);

                            break;
                    }

                    return (true, done, io);
                case UsbConst.USB_TYPE_CLASS:
                    Logger.DebugFormat("Ep0Request #USB_TYPE_CLASS:");

                    switch (@event.Request.bRequest)
                    {
                        case HidConst.HID_REQ_SET_REPORT:
                            io.Inner.Length = 1;
                            done = true;
                            break;
                        case HidConst.HID_REQ_SET_IDLE:
                            io.Inner.Length = 0;
                            break;
                        case HidConst.HID_REQ_SET_PROTOCOL:
                            io.Inner.Length = 0;
                            done = true;
                            break;
                        default:
                            Logger.DebugFormat("Ep0Request #USB_TYPE_CLASS: Unknow...");
                            break;
                    }
                    return (true, done, io);
                case UsbConst.USB_TYPE_VENDOR:
                    Logger.DebugFormat("Ep0Request #USB_TYPE_VENDOR:");
                    break;
                default:
                    Logger.DebugFormat("Ep0Request RequestType: Unknow");
                    break;
            }

            return (false, done, io);
        }

        private async Task<UsbRawControlIo> Reply(UsbRawControlEvent @event, UsbRawControlIo io)
        {
            UsbSetupPacket setupPacket = new UsbSetupPacket(@event.Request.bRequestType,
                @event.Request.bRequest,
                @event.Request.wValue,
                @event.Request.wIndex,
                @event.Request.wLength);

            byte[] buf = new byte[RawGadgetConst.EP0_MAX_DATA];

            Logger.DebugFormat("Ep0Request UsbSetupPacket Relay: {0}", setupPacket.RequestType);

            int uTransferLength = UsbDevice.ControlTransfer(setupPacket, buf, 0, buf.Length);

            Logger.DebugFormat("Ep0Request ControlTransfer: {0}", uTransferLength);

            io = await MakeUsbControlIo(buf.Take(uTransferLength).ToArray());
            return io;
        }
    }
}
