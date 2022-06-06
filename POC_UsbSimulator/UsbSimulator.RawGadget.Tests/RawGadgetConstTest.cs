using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using UsbSimulator.RawGadget.LowLevel;
using UsbSimulator.RawGadget.LowLevel.RawGadget;
using Xunit;

namespace UsbSimulator.RawGadget.Tests
{
    public class RawGadgetConstTest
    {
        [Fact]
        public void RawGadgetIOCTLTest()
        {
            Dictionary<string, uint> consts = new Dictionary<string, uint>()
            {
                { "USB_RAW_IOCTL_INIT", 1090606336 },
                { "USB_RAW_IOCTL_RUN", 21761 },
                { "USB_RAW_IOCTL_EVENT_FETCH", 2148029698 },
                { "USB_RAW_IOCTL_EP0_WRITE", 1074287875 },
                { "USB_RAW_IOCTL_EP0_READ", 3221771524 },
                { "USB_RAW_IOCTL_EP_ENABLE", 1074353413 },
                { "USB_RAW_IOCTL_EP_DISABLE", 1074025734 },
                { "USB_RAW_IOCTL_EP_WRITE", 1074287879 },
                { "USB_RAW_IOCTL_EP_READ", 3221771528 },
                { "USB_RAW_IOCTL_CONFIGURE", 21769 },
                { "USB_RAW_IOCTL_VBUS_DRAW", 1074025738 },
                { "USB_RAW_IOCTL_EPS_INFO", 2210419979 },
                { "USB_RAW_IOCTL_EP0_STALL", 21772 },
                { "USB_RAW_IOCTL_EP_SET_HALT", 1074025741 },
                { "USB_RAW_IOCTL_EP_CLEAR_HALT", 1074025742 },
                { "USB_RAW_IOCTL_EP_SET_WEDGE", 1074025743 },
            };

            foreach (var item in consts)
            {
                var @type = typeof(RawGadgetConst);
                var member = @type.FindMembers(MemberTypes.Method, BindingFlags.Static | BindingFlags.Public, null, null);
                var method = member.Where(x => x.Name.EndsWith(item.Key)).FirstOrDefault() as MethodInfo;

                Assert.NotNull(method);
                var output = (int)method.Invoke(null, new object[] { });

                Assert.EndsWith(item.Key, method.Name);
                Assert.Equal(item.Value, BitConverter.ToUInt32(BitConverter.GetBytes(output)));
            }
        }
    }
}
