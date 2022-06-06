using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Castle.Core.Logging;
using Tmds.Linux;
using UsbSimulator.RawGadget.LowLevel;
using UsbSimulator.RawGadget.LowLevel.RawGadget;
using UsbSimulator.RawGadget.LowLevel.Usb;
using System.Linq;
using LibUsbDotNet.Main;
using LibUsbDotNet;

namespace UsbSimulator.RawGadget
{
    public class UsbRawGadget : IDisposable
    {
        public UsbRawGadget()
            : this("/dev/raw-gadget", "dummy_udc", "dummy_udc.0")
        {
        }

        public UsbRawGadget(string driver, string device)
            : this("/dev/raw-gadget", driver, device)
        {
        }

        public UsbRawGadget(string path, string driver, string device)
        {
            SysIoctlDevice = path;
            UdcDriverName = driver;
            UdcDeviceName = device;

        }

        private int _fileDescriptor = 0;
        protected Dictionary<string, UsbStringDescriptor> StringDescriptor = new Dictionary<string, UsbStringDescriptor>();
        protected UsbRawEpsInfo epsInfo = new UsbRawEpsInfo();


        public ILogger Logger { get; set; } = NullLogger.Instance;

        public string SysIoctlDevice { get; private set; }
        public string UdcDriverName { get; private set; }
        public string UdcDeviceName { get; private set; }

        public UsbDeviceInfo DeviceInfo { get; set; } = new UsbDeviceInfo();

        private UsbDeviceDescriptor ProcessUsbDeviceDescriptor()
        {
            ProcessStringDescriptor();

            UsbDeviceDescriptor descriptor = new UsbDeviceDescriptor()
            {
                bLength = UsbConst.USB_DT_DEVICE_SIZE,
                bDescriptorType = UsbConst.USB_DT_DEVICE,
                bcdUSB = 0x0200,
                bDeviceClass = Convert.ToByte(DeviceInfo.DeviceClass),
                bDeviceSubClass = Convert.ToByte(DeviceInfo.DeviceSubClass),
                bDeviceProtocol = Convert.ToByte(DeviceInfo.DeviceProtocol),
                bMaxPacketSize0 = RawGadgetConst.EP_MAX_PACKET_CONTROL,
                idVendor = DeviceInfo.Vendor,
                idProduct = DeviceInfo.Product,
                bcdDevice = DeviceInfo.Version,
                iManufacturer = Convert.ToByte(Array.IndexOf(StringDescriptor.Keys.ToArray(), "Manufacturer")),
                iProduct = Convert.ToByte(Array.IndexOf(StringDescriptor.Keys.ToArray(), "ProductName")),
                iSerialNumber = Convert.ToByte(Array.IndexOf(StringDescriptor.Keys.ToArray(), "SerialNumber")),
                bNumConfigurations = 1,
            };

            return descriptor;
        }

        private UsbConfigDescriptor ProcessUsbConfigDescriptor()
        {
            UsbConfigDescriptor descriptor = new UsbConfigDescriptor()
            {
                bLength = UsbConst.USB_DT_CONFIG_SIZE,
                bDescriptorType = UsbConst.USB_DT_CONFIG,
            };

            return descriptor;
        }

        protected void ProcessStringDescriptor()
        {
            StringDescriptor.Clear();
            Dictionary<string, UsbStringDescriptor> _stringDescriptor = new Dictionary<string, UsbStringDescriptor>();

            _stringDescriptor.Add("Empty", new UsbStringDescriptor()
            {
                bDescriptorType = UsbConst.USB_DT_STRING,
                Data = Encoding.Unicode.GetBytes(string.Empty),
            });

            _stringDescriptor.Add("Manufacturer", new UsbStringDescriptor()
            {
                bDescriptorType = UsbConst.USB_DT_STRING,
                Data = Encoding.Unicode.GetBytes(DeviceInfo.Manufacturer),
            });

            _stringDescriptor.Add("ProductName", new UsbStringDescriptor()
            {
                bDescriptorType = UsbConst.USB_DT_STRING,
                Data = Encoding.Unicode.GetBytes(DeviceInfo.ProductName),
            });

            _stringDescriptor.Add("SerialNumber", new UsbStringDescriptor()
            {
                bDescriptorType = UsbConst.USB_DT_STRING,
                Data = Encoding.Unicode.GetBytes(DeviceInfo.SerialNumber),
            });

            foreach (var item in _stringDescriptor)
            {
                var _item = item.Value;
                _item.Data = _item.Data.PadRight(UsbConst.USB_MAX_STRING_LEN);
                _item.bLength = Convert.ToByte(_item.Data.Length + 2);

                StringDescriptor.Add(item.Key, _item);
            }
        }


        public virtual async Task Open()
        {
            Logger.DebugFormat("Open Raw Gadget...");
            unsafe
            {
                fixed (byte* _tmp = Encoding.UTF8.GetBytes(SysIoctlDevice))
                {
                    _fileDescriptor = LibC.open(_tmp, LibC.O_RDWR);
                }
            }

            if (_fileDescriptor < 0)
                throw new IOException("open()");

            Logger.DebugFormat("Opened Raw Gadget: {0}", _fileDescriptor);

            await Init();
            await Run();

            await Ep0Loop();
            await EpLoop();
        }

        protected virtual async Task Init()
        {
            UsbRawInit init = new UsbRawInit()
            {
                DriverName = Encoding.UTF8.GetBytes(UdcDriverName).PadRight(RawGadgetConst.UDC_NAME_LENGTH_MAX),
                DeviceName = Encoding.UTF8.GetBytes(UdcDeviceName).PadRight(RawGadgetConst.UDC_NAME_LENGTH_MAX),
                Speed = UsbDeviceSpeed.USB_SPEED_HIGH,
            };

            Logger.DebugFormat("Init Raw Gadget...");
            await Ioctl<UsbRawInit>(RawGadgetConst.USB_RAW_IOCTL_INIT, init);
        }

        protected async Task Run()
        {
            Logger.DebugFormat("Run Raw Gadget...");
            await Ioctl(RawGadgetConst.USB_RAW_IOCTL_RUN, 0);
        }

        protected virtual async Task EpLoop()
        {
            await Task.Yield();
        }

        protected async Task Ep0Loop()
        {
            Logger.DebugFormat("Run Ep0 loop...");

            bool done = false;

            while (!done)
            {
                try
                {
                    UsbRawControlEvent @event = await RawEventFetch();

                    //TODO: Event.
                    Logger.DebugFormat("Fetched USB Event {0}, Length: {1}", @event.Inner.EventType.ToString(), @event.Inner.Length);

                    if (@event.Inner.EventType == UsbRawEventType.USB_RAW_EVENT_CONNECT)
                    {
                        await ProcessEpsInfo();
                    }

                    if (@event.Inner.EventType != UsbRawEventType.USB_RAW_EVENT_CONTROL)
                    {
                        continue;
                    }

                    bool reply = false;
                    UsbRawControlIo io = new UsbRawControlIo();
                    (reply, done, io) = await Ep0Request(@event, done);

                    if (!reply)
                    {
                        await Ep0Stall();
                        continue;
                    }

                    if (@event.Request.wLength < io.Inner.Length)
                    {
                        io.Inner.Length = @event.Request.wLength;
                    }

                    int rv = -1;

                    if ((@event.Request.bRequestType & UsbConst.USB_DIR_IN) != 0)
                    {
                        await Ep0Write<UsbRawControlIo>(io);
                    }
                    else
                    {
                        await Ep0Read<UsbRawControlIo>(io);
                    }

                    Logger.DebugFormat("Fetched USB Event {0} Done", @event.Inner.EventType.ToString(), @event.Inner.Length);

                }
                catch (Exception ex)
                {
                    //done = true;
                    Logger.ErrorFormat("Fetched USB Event Error, {0}\n{1}", ex.Message, ex.StackTrace);
                }

            }

            Logger.DebugFormat("Run Ep0 loop done.");
        }


        protected async Task ProcessEpsInfo()
        {
            int rv = -1;

            (rv, epsInfo) = await EpsInfo();

            for (int i = 0; i < rv; i++)
            {
                var item = epsInfo.Eps[i];

                Logger.DebugFormat("Endpoint #{0}", i);
                Logger.DebugFormat("\t Name: {0}", Encoding.UTF8.GetString(item.Name));
                Logger.DebugFormat("\t Addr: {0}", item.Address);
                Logger.DebugFormat("\t Type: {0}", item.Caps.Enum);
                Logger.DebugFormat("\t MaxPacketLimit: {0}", item.Limits.MaxpacketLimit);
                Logger.DebugFormat("\t MaxStreams: {0}", item.Limits.MaxStreams);
                Logger.DebugFormat("\t Reserved: {0}", item.Limits.Reserved);
            }

            for (int i = 0; i < rv; i++)
            {
                var item = epsInfo.Eps[i];

                bool flag = false;
                UsbEndpointDescriptor descriptor = new UsbEndpointDescriptor()
                {
                    bLength = UsbConst.USB_DT_ENDPOINT_SIZE,
                    bDescriptorType = UsbConst.USB_DT_ENDPOINT,
                    bEndpointAddress = UsbConst.USB_DIR_IN | 0x00,
                    bmAttributes = UsbConst.USB_ENDPOINT_XFER_INT,
                    wMaxPacketSize = RawGadgetConst.EP_MAX_PACKET_INT,
                    bInterval = 5,
                };

                (flag, descriptor) = await AssignEpAddress(item, descriptor);

                //TODO: AssignEpAddress
            }
        }

        protected async Task<ValueTuple<bool, UsbEndpointDescriptor>> AssignEpAddress(UsbRawEpInfo info, UsbEndpointDescriptor endpointDescriptor)
        {
            Logger.DebugFormat("Assign Ep Address Start...");

            if (UsbConst.usb_endpoint_num(endpointDescriptor) != 0)
                return (false, endpointDescriptor);

            if (UsbConst.usb_endpoint_dir_in(endpointDescriptor) && !info.Caps.Enum.HasFlag(UsbRawEpCapsEnum.DirIn))
                return (false, endpointDescriptor);

            if (UsbConst.usb_endpoint_dir_out(endpointDescriptor) && !info.Caps.Enum.HasFlag(UsbRawEpCapsEnum.DirOut))
                return (false, endpointDescriptor);

            switch (UsbConst.usb_endpoint_type(endpointDescriptor))
            {
                case UsbConst.USB_ENDPOINT_XFER_BULK:
                    if (!info.Caps.Enum.HasFlag(UsbRawEpCapsEnum.TypeBulk))
                        return (false, endpointDescriptor);
                    break;
                case UsbConst.USB_ENDPOINT_XFER_INT:
                    if (!info.Caps.Enum.HasFlag(UsbRawEpCapsEnum.TypeInt))
                        return (false, endpointDescriptor);
                    break;
                default:
                    throw new NotSupportedException("Unknow endpoint type.");
            }

            if (info.Address == RawGadgetConst.USB_RAW_EP_ADDR_ANY)
            {
                uint addr = 1;
                endpointDescriptor.bEndpointAddress |= Convert.ToByte(addr++);
            }
            else
            {
                endpointDescriptor.bEndpointAddress |= Convert.ToByte(info.Address);
            }

            await Task.Yield();

            Logger.DebugFormat("Assign Ep Address End...");

            return (true, endpointDescriptor);
        }


        protected virtual async Task<ValueTuple<bool, bool, UsbRawControlIo>> Ep0Request(UsbRawControlEvent @event, bool done)
        {
            await Task.Yield();

            UsbRawControlIo io = new UsbRawControlIo();

            Logger.DebugFormat("Ep0Request RequestType: {0}", @event.Request.bRequestType & UsbConst.USB_TYPE_MASK);
            switch (@event.Request.bRequestType & UsbConst.USB_TYPE_MASK)
            {
                case UsbConst.USB_TYPE_STANDARD:
                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD, REQ: {0}", @event.Request.bRequest);
                    switch (@event.Request.bRequest)
                    {
                        case UsbConst.USB_REQ_GET_DESCRIPTOR:
                            Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_DESCRIPTOR, Value: {0}", @event.Request.wValue >> 8);
                            switch (@event.Request.wValue >> 8)
                            {
                                case UsbConst.USB_DT_DEVICE:
                                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_DESCRIPTOR:#USB_DT_DEVICE");
                                    io = await MakeUsbControlIo<UsbDeviceDescriptor>(ProcessUsbDeviceDescriptor());
                                    return (true, done, io);
                                case UsbConst.USB_DT_DEVICE_QUALIFIER:
                                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_DESCRIPTOR:#USB_DT_DEVICE_QUALIFIER");
                                    break;
                                case UsbConst.USB_DT_CONFIG:
                                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_DESCRIPTOR:#USB_DT_CONFIG");
                                    break;
                                case UsbConst.USB_DT_STRING:
                                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_DESCRIPTOR:#USB_DT_STRING, wIndex:{0}, wValue:{1}", @event.Request.wIndex, BitConverter.GetBytes(@event.Request.wValue).FirstOrDefault());
                                    io = await MakeUsbControlIo<UsbStringDescriptor>(StringDescriptor.ElementAt(BitConverter.GetBytes(@event.Request.wValue).FirstOrDefault()).Value);
                                    return (true, done, io);
                                default:
                                    Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_DESCRIPTOR Unknow");
                                    break;
                            }
                            break;
                        case UsbConst.USB_REQ_SET_CONFIGURATION:
                            Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_SET_CONFIGURATION");
                            break;
                        case UsbConst.USB_REQ_GET_INTERFACE:
                            Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD:#USB_REQ_GET_INTERFACE");
                            break;
                        default:
                            Logger.DebugFormat("Ep0Request #USB_TYPE_STANDARD, REQ Unknow");
                            break;
                    }
                    break;
                case UsbConst.USB_TYPE_CLASS:
                    Logger.DebugFormat("Ep0Request #USB_TYPE_CLASS:");
                    break;
                case UsbConst.USB_TYPE_VENDOR:
                    Logger.DebugFormat("Ep0Request #USB_TYPE_VENDOR:");
                    break;
                default:
                    Logger.DebugFormat("Ep0Request RequestType: Unknow");
                    break;
            }

            return (false, done, io);
        }

        protected async Task<int> EnableEp(UsbEndpointDescriptor descriptor)
        {
            Logger.DebugFormat("Enable Ep...");
            int rv = -1;

            (rv, _) = await Ioctl(RawGadgetConst.USB_RAW_IOCTL_EP_ENABLE, descriptor);
            return rv;
        }

        protected async Task EpWrite<T>(T io)
            where T : struct
        {
            Logger.DebugFormat("Ep Write...");

            await Ioctl<T>(RawGadgetConst.USB_RAW_IOCTL_EP_WRITE, io);
        }

        protected async Task Ep0Stall()
        {
            Logger.DebugFormat("Ep0 Stall...");

            await Ioctl(RawGadgetConst.USB_RAW_IOCTL_EP0_STALL, 0);
            await Task.Yield();
        }

        protected async Task<T> Ep0Write<T>(T obj)
            where T : struct
        {
            Logger.DebugFormat("Ep0 Write...");
            int rv = -1;

            (rv, obj) = await Ioctl<T>(RawGadgetConst.USB_RAW_IOCTL_EP0_WRITE, obj);

            Logger.DebugFormat("Ep0 transferred {0} bytes IN.", rv);

            return obj;
        }

        protected async Task<T> Ep0Read<T>(T obj)
            where T : struct
        {
            Logger.DebugFormat("Ep0 Read...");
            int rv = -1;

            (rv, obj) = await Ioctl<T>(RawGadgetConst.USB_RAW_IOCTL_EP0_READ, obj);

            Logger.DebugFormat("Ep0 transferred {0} bytes OUT.", rv);

            return obj;
        }

        protected async Task<(int, UsbRawEpsInfo)> EpsInfo()
        {
            return await Ioctl<UsbRawEpsInfo>(RawGadgetConst.USB_RAW_IOCTL_EPS_INFO, new UsbRawEpsInfo());
        }

        protected async Task<int> VbusDraw(int power)
        {
            return await Ioctl(RawGadgetConst.USB_RAW_IOCTL_VBUS_DRAW, power);
        }

        protected async Task<int> Configure()
        {
            return await Ioctl(RawGadgetConst.USB_RAW_IOCTL_CONFIGURE, 0);
        }

        protected async Task<UsbRawControlEvent> RawEventFetch()
        {
            var data = await Ioctl<UsbRawControlEvent>(RawGadgetConst.USB_RAW_IOCTL_EVENT_FETCH, new UsbRawControlEvent()
            {
                Inner = new UsbRawEvent()
                {
                    Length = Convert.ToUInt32(Marshal.SizeOf<UsbCtrlRequest>()),
                }
            });

            return data.Item2;
        }

        protected async Task<int> Ioctl(int cmd, int data)
        {
            int rv = -1;

            Logger.DebugFormat("Ioctl Start, Cmd: {0}, Data: {1}", cmd, data);

            unsafe
            {
                rv = LibC.ioctl(_fileDescriptor, cmd, data);

                if (rv < 0)
                    throw new IOException($"ioctl({cmd})");
            }

            Logger.DebugFormat("Ioctl End, Cmd: {0}, Data: {1}, Rv: {2}", cmd, data, rv);
            await Task.Yield();

            return rv;
        }

        protected async Task<int> Ioctl(int cmd, IntPtr ptr)
        {
            int rv = -1;

            Logger.DebugFormat("Ioctl Start, Cmd: {0}, Ptr: {1}", cmd, ptr.ToInt64());

            unsafe
            {
                rv = LibC.ioctl(_fileDescriptor, cmd, ptr.ToPointer());

                if (rv < 0)
                    throw new IOException($"ioctl({cmd})");
            }

            Logger.DebugFormat("Ioctl End, Cmd: {0}, Ptr: {1}, Rv: {2}", cmd, ptr.ToInt64(), rv);
            await Task.Yield();

            return rv;
        }

        protected async Task<ValueTuple<int, T>> Ioctl<T>(int cmd, T data)
            where T : struct
        {

            IntPtr intPtr = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
            Marshal.StructureToPtr<T>(data, intPtr, true);

            Logger.DebugFormat("Ioctl Write Structure, Cmd: {0}, Structute: {1}, Size: {2}", cmd, typeof(T).Name, Marshal.SizeOf<T>());

            int rv = await Ioctl(cmd, intPtr);

            data = Marshal.PtrToStructure<T>(intPtr);
            Marshal.DestroyStructure<UsbRawControlEvent>(intPtr);//销毁

            Logger.DebugFormat("Ioctl Read Structure, Cmd: {0}, Structute: {1}, Size: {2}", cmd, typeof(T).Name, Marshal.SizeOf<T>());

            return (rv, data);
        }

        protected async Task<UsbRawControlIo> MakeUsbControlIo(byte[] data)
        {

            UsbRawControlIo io = new UsbRawControlIo()
            {
                Inner = new UsbRawEpIo()
                {
                    Ep = 0,
                    Flags = 0,
                    Length = Convert.ToUInt32(data.Length),
                },
                Data = data.PadRight(RawGadgetConst.EP0_MAX_DATA),
            };

            await Task.Yield();

            return io;
        }

        protected async Task<UsbRawControlIo> MakeUsbControlIo<T>(T obj)
        {
            byte[] data = await StructureToByteArray<T>(obj);

            UsbRawControlIo io = new UsbRawControlIo()
            {
                Inner = new UsbRawEpIo()
                {
                    Ep = 0,
                    Flags = 0,
                    Length = Convert.ToUInt32(Marshal.SizeOf<T>()),
                },
                Data = data.PadRight(RawGadgetConst.EP0_MAX_DATA),
            };

            await Task.Yield();

            return io;
        }

        protected async Task<byte[]> StructureToByteArray<T>(T obj)
        {
            int len = Marshal.SizeOf<T>();
            byte[] arr = new byte[len];

            IntPtr ptr = Marshal.AllocHGlobal(len);

            Marshal.StructureToPtr(obj, ptr, true);
            Marshal.Copy(ptr, arr, 0, len);

            Marshal.DestroyStructure<T>(ptr);

            await Task.Yield();
            return arr;
        }

        protected async Task<T> ByteArrayToStructure<T>(byte[] bytearray)
        {
            T obj = default(T);
            int len = Marshal.SizeOf<T>();
            IntPtr i = Marshal.AllocHGlobal(len);

            Marshal.Copy(bytearray, 0, i, len);

            Marshal.PtrToStructure<T>(i);

            Marshal.DestroyStructure<T>(i);

            await Task.Yield();
            return obj;
        }



        public void Dispose()
        {
            if (_fileDescriptor > 0)
            {
                unsafe
                {
                    LibC.close(_fileDescriptor);
                }
            }
        }
    }
}
