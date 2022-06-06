using System;
using UsbSimulator.RawGadget.LowLevel.RawGadget;
using UsbSimulator.RawGadget.LowLevel.Usb;

namespace UsbSimulator.RawGadget.LowLevel
{
    public static class RawGadgetConst
    {
        /* Maximum length of driver_name/device_name in the usb_raw_init struct. */
        public const int UDC_NAME_LENGTH_MAX = 128;

        public const int USB_RAW_IO_FLAGS_ZERO = 0x0001;
        public const int USB_RAW_IO_FLAGS_MASK = 0x0001;

        /* Maximum number of non-control endpoints in struct usb_raw_eps_info. */
        public const int USB_RAW_EPS_NUM_MAX = 30;

        /* Maximum length of UDC endpoint name in struct usb_raw_ep_info. */
        public const int USB_RAW_EP_NAME_MAX = 16;

        /* Used as addr in struct usb_raw_ep_info if endpoint accepts any address. */
        public const int USB_RAW_EP_ADDR_ANY = 0xff;

        /*
         * Initializes a Raw Gadget instance.
         * Accepts a pointer to the usb_raw_init struct as an argument.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_INIT => Ioctl._IOW<UsbRawInit>('U', 0);

        /*
         * Instructs Raw Gadget to bind to a UDC and start emulating a USB device.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_RUN => Ioctl._IO('U', 1);

        /*
         * A blocking ioctl that waits for an event and returns fetched event data to
         * the user.
         * Accepts a pointer to the usb_raw_event struct.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_EVENT_FETCH => Ioctl._IOR<UsbRawEvent>('U', 2);

        /*
         * Queues an IN (OUT for READ) request as a response to the last setup request
         * received on endpoint 0 (provided that was an IN (OUT for READ) request), and
         * waits until the request is completed. Copies received data to user for READ.
         * Accepts a pointer to the usb_raw_ep_io struct as an argument.
         * Returns length of transferred data on success or negative error code on
         * failure.
         */
        public static int USB_RAW_IOCTL_EP0_WRITE => Ioctl._IOW<UsbRawEpIo>('U', 3);
        public static int USB_RAW_IOCTL_EP0_READ => Ioctl._IOWR<UsbRawEpIo>('U', 4);

        /*
         * Finds an endpoint that satisfies the parameters specified in the provided
         * descriptors (address, transfer type, etc.) and enables it.
         * Accepts a pointer to the usb_raw_ep_descs struct as an argument.
         * Returns enabled endpoint handle on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_EP_ENABLE => Ioctl._IOW<UsbEndpointDescriptor>('U', 5);

        /*
         * Disables specified endpoint.
         * Accepts endpoint handle as an argument.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_EP_DISABLE => Ioctl._IOW<int>('U', 6);

        /*
         * Queues an IN (OUT for READ) request as a response to the last setup request
         * received on endpoint usb_raw_ep_io.ep (provided that was an IN (OUT for READ)
         * request), and waits until the request is completed. Copies received data to
         * user for READ.
         * Accepts a pointer to the usb_raw_ep_io struct as an argument.
         * Returns length of transferred data on success or negative error code on
         * failure.
         */
        public static int USB_RAW_IOCTL_EP_WRITE => Ioctl._IOW<UsbRawEpIo>('U', 7);
        public static int USB_RAW_IOCTL_EP_READ => Ioctl._IOWR<UsbRawEpIo>('U', 8);

        /*
         * Switches the gadget into the configured state.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_CONFIGURE => Ioctl._IO('U', 9);

        /*
         * Constrains UDC VBUS power usage.
         * Accepts current limit in 2 mA units as an argument.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_VBUS_DRAW => Ioctl._IOW<int>('U', 10);

        /*
         * Fills in the usb_raw_eps_info structure with information about non-control
         * endpoints available for the currently connected UDC.
         * Returns the number of available endpoints on success or negative error code
         * on failure.
         */
        public static int USB_RAW_IOCTL_EPS_INFO => Ioctl._IOR<UsbRawEpsInfo>('U', 11);

        /*
         * Stalls a pending control request on endpoint 0.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_EP0_STALL => Ioctl._IO('U', 12);

        /*
         * Sets or clears halt or wedge status of the endpoint.
         * Accepts endpoint handle as an argument.
         * Returns 0 on success or negative error code on failure.
         */
        public static int USB_RAW_IOCTL_EP_SET_HALT => Ioctl._IOW<int>('U', 13);
        public static int USB_RAW_IOCTL_EP_CLEAR_HALT => Ioctl._IOW<int>('U', 14);
        public static int USB_RAW_IOCTL_EP_SET_WEDGE => Ioctl._IOW<int>('U', 15);

        public const int EP_MAX_PACKET_CONTROL = 64;
        public const int EP_MAX_PACKET_INT = 8;

        // Assigned dynamically.
        public const int EP_NUM_INT_IN = 0x0;
        public const int EP0_MAX_DATA = 256;
    }
}
