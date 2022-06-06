using System;
using UsbSimulator.RawGadget.LowLevel.Usb;

namespace UsbSimulator.RawGadget.LowLevel
{
    public class UsbConst
    {
        /*
         * USB directions
         *
         * This bit flag is used in endpoint descriptors' bEndpointAddress field.
         * It's also one of three fields in control requests bRequestType.
         */
        public const int USB_DIR_OUT = 0;              /* to device */
        public const int USB_DIR_IN = 0x80;            /* to host */

        /*
         * USB types, the second of three bRequestType fields
         */
        public const int USB_TYPE_MASK = (0x03 << 5);
        public const int USB_TYPE_STANDARD = (0x00 << 5);
        public const int USB_TYPE_CLASS = (0x01 << 5);
        public const int USB_TYPE_VENDOR = (0x02 << 5);
        public const int USB_TYPE_RESERVED = (0x03 << 5);

        /*
         * USB recipients, the third of three bRequestType fields
         */
        public const int USB_RECIP_MASK = 0x1f;
        public const int USB_RECIP_DEVICE = 0x00;
        public const int USB_RECIP_INTERFACE = 0x01;
        public const int USB_RECIP_ENDPOINT = 0x02;
        public const int USB_RECIP_OTHER = 0x03;
        /* From Wireless USB 1.0 */
        public const int USB_RECIP_PORT = 0x04;
        public const int USB_RECIP_RPIPE = 0x05;

        /*
         * Standard requests, for the bRequest field of a SETUP packet.
         *
         * These are qualified by the bRequestType field, so that for example
         * TYPE_CLASS or TYPE_VENDOR specific feature flags could be retrieved
         * by a GET_STATUS request.
         */
        public const int USB_REQ_GET_STATUS = 0x00;
        public const int USB_REQ_CLEAR_FEATURE = 0x01;
        public const int USB_REQ_SET_FEATURE = 0x03;
        public const int USB_REQ_SET_ADDRESS = 0x05;
        public const int USB_REQ_GET_DESCRIPTOR = 0x06;
        public const int USB_REQ_SET_DESCRIPTOR = 0x07;
        public const int USB_REQ_GET_CONFIGURATION = 0x08;
        public const int USB_REQ_SET_CONFIGURATION = 0x09;
        public const int USB_REQ_GET_INTERFACE = 0x0A;
        public const int USB_REQ_SET_INTERFACE = 0x0B;
        public const int USB_REQ_SYNCH_FRAME = 0x0C;
        public const int USB_REQ_SET_SEL = 0x30;
        public const int USB_REQ_SET_ISOCH_DELAY = 0x31;

        public const int USB_REQ_SET_ENCRYPTION = 0x0D;    /* Wireless USB */
        public const int USB_REQ_GET_ENCRYPTION = 0x0E;
        public const int USB_REQ_RPIPE_ABORT = 0x0E;
        public const int USB_REQ_SET_HANDSHAKE = 0x0F;
        public const int USB_REQ_RPIPE_RESET = 0x0F;
        public const int USB_REQ_GET_HANDSHAKE = 0x10;
        public const int USB_REQ_SET_CONNECTION = 0x11;
        public const int USB_REQ_SET_SECURITY_DATA = 0x12;
        public const int USB_REQ_GET_SECURITY_DATA = 0x13;
        public const int USB_REQ_SET_WUSB_DATA = 0x14;
        public const int USB_REQ_LOOPBACK_DATA_WRITE = 0x15;
        public const int USB_REQ_LOOPBACK_DATA_READ = 0x16;
        public const int USB_REQ_SET_INTERFACE_DS = 0x17;

        /* specific requests for USB Power Delivery */
        public const int USB_REQ_GET_PARTNER_PDO = 20;
        public const int USB_REQ_GET_BATTERY_STATUS = 21;
        public const int USB_REQ_SET_PDO = 22;
        public const int USB_REQ_GET_VDM = 23;
        public const int USB_REQ_SEND_VDM = 24;

        /* The Link Power Management (LPM) ECN defines USB_REQ_TEST_AND_SET command,
         * used by hubs to put ports into a new L1 suspend state, except that it
         * forgot to define its number ...
         */

        /*
         * USB feature flags are written using USB_REQ_{CLEAR,SET}_FEATURE, and
         * are read as a bit array returned by USB_REQ_GET_STATUS.  (So there
         * are at most sixteen features of each type.)  Hubs may also support a
         * new USB_REQ_TEST_AND_SET_FEATURE to put ports into L1 suspend.
         */
        public const int USB_DEVICE_SELF_POWERED = 0;       /* (read only) */
        public const int USB_DEVICE_REMOTE_WAKEUP = 1;       /* dev may initiate wakeup */
        public const int USB_DEVICE_TEST_MODE = 2;       /* (wired high speed only) */
        public const int USB_DEVICE_BATTERY = 2;       /* (wireless) */
        public const int USB_DEVICE_B_HNP_ENABLE = 3;       /* (otg) dev may initiate HNP */
        public const int USB_DEVICE_WUSB_DEVICE = 3;       /* (wireless)*/
        public const int USB_DEVICE_A_HNP_SUPPORT = 4;       /* (otg) RH port supports HNP */
        public const int USB_DEVICE_A_ALT_HNP_SUPPORT = 5;       /* (otg) other RH port does */
        public const int USB_DEVICE_DEBUG_MODE = 6;       /* (special devices only) */

        /*
         * Test Mode Selectors
         * See USB 2.0 spec Table 9-7
         */
        public const int USB_TEST_J = 1;
        public const int USB_TEST_K = 2;
        public const int USB_TEST_SE0_NAK = 3;
        public const int USB_TEST_PACKET = 4;
        public const int USB_TEST_FORCE_ENABLE = 5;

        /* Status Type */
        public const int USB_STATUS_TYPE_STANDARD = 0;
        public const int USB_STATUS_TYPE_PTM = 1;

        /*
         * Descriptor types ... USB 2.0 spec table 9.5
         */
        public const int USB_DT_DEVICE = 0x01;
        public const int USB_DT_CONFIG = 0x02;
        public const int USB_DT_STRING = 0x03;
        public const int USB_DT_INTERFACE = 0x04;
        public const int USB_DT_ENDPOINT = 0x05;
        public const int USB_DT_DEVICE_QUALIFIER = 0x06;
        public const int USB_DT_OTHER_SPEED_CONFIG = 0x07;
        public const int USB_DT_INTERFACE_POWER = 0x08;
        /* these are from a minor usb 2.0 revision (ECN) */
        public const int USB_DT_OTG = 0x09;
        public const int USB_DT_DEBUG = 0x0a;
        public const int USB_DT_INTERFACE_ASSOCIATION = 0x0b;
        /* these are from the Wireless USB spec */
        public const int USB_DT_SECURITY = 0x0c;
        public const int USB_DT_KEY = 0x0d;
        public const int USB_DT_ENCRYPTION_TYPE = 0x0e;
        public const int USB_DT_BOS = 0x0f;
        public const int USB_DT_DEVICE_CAPABILITY = 0x10;
        public const int USB_DT_WIRELESS_ENDPOINT_COMP = 0x11;
        public const int USB_DT_WIRE_ADAPTER = 0x21;
        public const int USB_DT_RPIPE = 0x22;
        public const int USB_DT_CS_RADIO_CONTROL = 0x23;
        /* From the T10 UAS specification */
        public const int USB_DT_PIPE_USAGE = 0x24;
        /* From the USB 3.0 spec */
        public const int USB_DT_SS_ENDPOINT_COMP = 0x30;
        /* From the USB 3.1 spec */
        public const int USB_DT_SSP_ISOC_ENDPOINT_COMP = 0x31;

        /* Conventional codes for class-specific descriptors.  The convention is
         * defined in the USB "Common Class" Spec (3.11).  Individual class specs
         * are authoritative for their usage, not the "common class" writeup.
         */
        public const int USB_DT_CS_DEVICE = (USB_TYPE_CLASS | USB_DT_DEVICE);
        public const int USB_DT_CS_CONFIG = (USB_TYPE_CLASS | USB_DT_CONFIG);
        public const int USB_DT_CS_STRING = (USB_TYPE_CLASS | USB_DT_STRING);
        public const int USB_DT_CS_INTERFACE = (USB_TYPE_CLASS | USB_DT_INTERFACE);
        public const int USB_DT_CS_ENDPOINT = (USB_TYPE_CLASS | USB_DT_ENDPOINT);

        public const int USB_DT_DEVICE_SIZE = 18;

        /*
         * Device and/or Interface Class codes
         * as found in bDeviceClass or bInterfaceClass
         * and defined by www.usb.org documents
         */
        public const int USB_CLASS_PER_INTERFACE = 0;       /* for DeviceClass */
        public const int USB_CLASS_AUDIO = 1;
        public const int USB_CLASS_COMM = 2;
        public const int USB_CLASS_HID = 3;
        public const int USB_CLASS_PHYSICAL = 5;
        public const int USB_CLASS_STILL_IMAGE = 6;
        public const int USB_CLASS_PRINTER = 7;
        public const int USB_CLASS_MASS_STORAGE = 8;
        public const int USB_CLASS_HUB = 9;
        public const int USB_CLASS_CDC_DATA = 0x0a;
        public const int USB_CLASS_CSCID = 0x0b;    /* chip+ smart card */
        public const int USB_CLASS_CONTENT_SEC = 0x0d;    /* content security */
        public const int USB_CLASS_VIDEO = 0x0e;
        public const int USB_CLASS_WIRELESS_CONTROLLER = 0xe0;
        public const int USB_CLASS_PERSONAL_HEALTHCARE = 0x0f;
        public const int USB_CLASS_AUDIO_VIDEO = 0x10;
        public const int USB_CLASS_BILLBOARD = 0x11;
        public const int USB_CLASS_USB_TYPE_C_BRIDGE = 0x12;
        public const int USB_CLASS_MISC = 0xef;
        public const int USB_CLASS_APP_SPEC = 0xfe;
        public const int USB_CLASS_VENDOR_SPEC = 0xff;

        public const int USB_SUBCLASS_VENDOR_SPEC = 0xff;

        public const int USB_DT_CONFIG_SIZE = 9;

        /* from config descriptor bmAttributes */
        public const int USB_CONFIG_ATT_ONE = (1 << 7);        /* must be set */
        public const int USB_CONFIG_ATT_SELFPOWER = (1 << 6);        /* self powered */
        public const int USB_CONFIG_ATT_WAKEUP = (1 << 5);        /* can wakeup */
        public const int USB_CONFIG_ATT_BATTERY = (1 << 4);        /* battery powered */

        /*-------------------------------------------------------------------------*/

        /* USB String descriptors can contain at most 126 characters. */
        public const int USB_MAX_STRING_LEN = 126;

        public const int USB_DT_INTERFACE_SIZE = 9;

        public const int USB_DT_ENDPOINT_SIZE = 7;
        public const int USB_DT_ENDPOINT_AUDIO_SIZE = 9;       /* Audio extension */

        /*
         * Endpoints
         */
        public const int USB_ENDPOINT_NUMBER_MASK = 0x0f;    /* in bEndpointAddress */
        public const int USB_ENDPOINT_DIR_MASK = 0x80;

        public const int USB_ENDPOINT_XFERTYPE_MASK = 0x03;    /* in bmAttributes */
        public const int USB_ENDPOINT_XFER_CONTROL = 0;
        public const int USB_ENDPOINT_XFER_ISOC = 1;
        public const int USB_ENDPOINT_XFER_BULK = 2;
        public const int USB_ENDPOINT_XFER_INT = 3;
        public const int USB_ENDPOINT_MAX_ADJUSTABLE = 0x80;

        public const int USB_ENDPOINT_MAXP_MASK = 0x07ff;
        public const int USB_EP_MAXP_MULT_SHIFT = 11;
        public const int USB_EP_MAXP_MULT_MASK = (3 << USB_EP_MAXP_MULT_SHIFT);
        public static int USB_EP_MAXP_MULT(int m) => (((m) & USB_EP_MAXP_MULT_MASK) >> USB_EP_MAXP_MULT_SHIFT);

        /* The USB 3.0 spec redefines bits 5:4 of bmAttributes as interrupt ep type. */
        public const int USB_ENDPOINT_INTRTYPE = 0x30;
        public const int USB_ENDPOINT_INTR_PERIODIC = (0 << 4);
        public const int USB_ENDPOINT_INTR_NOTIFICATION = (1 << 4);

        public const int USB_ENDPOINT_SYNCTYPE = 0x0c;
        public const int USB_ENDPOINT_SYNC_NONE = (0 << 2);
        public const int USB_ENDPOINT_SYNC_ASYNC = (1 << 2);
        public const int USB_ENDPOINT_SYNC_ADAPTIVE = (2 << 2);
        public const int USB_ENDPOINT_SYNC_SYNC = (3 << 2);

        public const int USB_ENDPOINT_USAGE_MASK = 0x30;
        public const int USB_ENDPOINT_USAGE_DATA = 0x00;
        public const int USB_ENDPOINT_USAGE_FEEDBACK = 0x10;
        public const int USB_ENDPOINT_USAGE_IMPLICIT_FB = 0x20;    /* Implicit feedback Data endpoint */

        /**
         * usb_endpoint_num - get the endpoint's number
         * @epd: endpoint to be checked
         *
         * Returns @epd's number: 0 to 15.
         */
        public static int usb_endpoint_num(UsbEndpointDescriptor descriptor) => descriptor.bEndpointAddress & USB_ENDPOINT_NUMBER_MASK;

        /**
         * usb_endpoint_type - get the endpoint's transfer type
         * @epd: endpoint to be checked
         *
         * Returns one of USB_ENDPOINT_XFER_{CONTROL, ISOC, BULK, INT} according
         * to @epd's transfer type.
         */
        public static int usb_endpoint_type(UsbEndpointDescriptor descriptor) => descriptor.bmAttributes & USB_ENDPOINT_XFERTYPE_MASK;

        /**
         * usb_endpoint_dir_in - check if the endpoint has IN direction
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint is of type IN, otherwise it returns false.
         */
        public static bool usb_endpoint_dir_in(UsbEndpointDescriptor descriptor) => ((descriptor.bEndpointAddress & USB_ENDPOINT_DIR_MASK) == USB_DIR_IN);

        /**
         * usb_endpoint_dir_out - check if the endpoint has OUT direction
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint is of type OUT, otherwise it returns false.
         */
        public static bool usb_endpoint_dir_out(UsbEndpointDescriptor descriptor) => ((descriptor.bEndpointAddress & USB_ENDPOINT_DIR_MASK) == USB_DIR_OUT);

        /**
         * usb_endpoint_xfer_bulk - check if the endpoint has bulk transfer type
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint is of type bulk, otherwise it returns false.
         */
        public static bool usb_endpoint_xfer_bulk(UsbEndpointDescriptor descriptor) => ((descriptor.bmAttributes & USB_ENDPOINT_XFERTYPE_MASK) ==
                USB_ENDPOINT_XFER_BULK);

        /**
         * usb_endpoint_xfer_control - check if the endpoint has control transfer type
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint is of type control, otherwise it returns false.
         */
        public static bool usb_endpoint_xfer_control(UsbEndpointDescriptor descriptor) => ((descriptor.bmAttributes & USB_ENDPOINT_XFERTYPE_MASK) ==
                USB_ENDPOINT_XFER_CONTROL);

        /**
         * usb_endpoint_xfer_int - check if the endpoint has interrupt transfer type
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint is of type interrupt, otherwise it returns
         * false.
         */
        public static bool usb_endpoint_xfer_int(UsbEndpointDescriptor descriptor) => ((descriptor.bmAttributes & USB_ENDPOINT_XFERTYPE_MASK) ==
                USB_ENDPOINT_XFER_INT);

        /**
         * usb_endpoint_xfer_isoc - check if the endpoint has isochronous transfer type
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint is of type isochronous, otherwise it returns
         * false.
         */
        public static bool usb_endpoint_xfer_isoc(UsbEndpointDescriptor descriptor) => ((descriptor.bmAttributes & USB_ENDPOINT_XFERTYPE_MASK) ==
                USB_ENDPOINT_XFER_ISOC);

        /**
         * usb_endpoint_is_bulk_in - check if the endpoint is bulk IN
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint has bulk transfer type and IN direction,
         * otherwise it returns false.
         */
        public static bool usb_endpoint_is_bulk_in(UsbEndpointDescriptor descriptor) => usb_endpoint_xfer_bulk(descriptor) && usb_endpoint_dir_in(descriptor);


        /**
         * usb_endpoint_is_bulk_out - check if the endpoint is bulk OUT
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint has bulk transfer type and OUT direction,
         * otherwise it returns false.
         */
        public static bool usb_endpoint_is_bulk_out(UsbEndpointDescriptor descriptor) => usb_endpoint_xfer_bulk(descriptor) && usb_endpoint_dir_out(descriptor);


        /**
         * usb_endpoint_is_int_in - check if the endpoint is interrupt IN
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint has interrupt transfer type and IN direction,
         * otherwise it returns false.
         */
        public static bool usb_endpoint_is_int_in(UsbEndpointDescriptor descriptor) => usb_endpoint_xfer_int(descriptor) && usb_endpoint_dir_in(descriptor);


        /**
         * usb_endpoint_is_int_out - check if the endpoint is interrupt OUT
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint has interrupt transfer type and OUT direction,
         * otherwise it returns false.
         */
        public static bool usb_endpoint_is_int_out(UsbEndpointDescriptor descriptor) => usb_endpoint_xfer_int(descriptor) && usb_endpoint_dir_out(descriptor);


        /**
         * usb_endpoint_is_isoc_in - check if the endpoint is isochronous IN
         * @epd: endpoint to be checked
         *
         * Returns true if the endpoint has isochronous transfer type and IN direction,
         * otherwise it returns false.
         */
        public static bool usb_endpoint_is_isoc_in(UsbEndpointDescriptor descriptor) => usb_endpoint_xfer_isoc(descriptor) && usb_endpoint_dir_in(descriptor);

    }
}
