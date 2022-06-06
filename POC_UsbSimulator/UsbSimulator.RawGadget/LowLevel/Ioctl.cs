using System;
using System.Runtime.InteropServices;

namespace UsbSimulator.RawGadget.LowLevel
{
    public static class Ioctl
    {
        /* ioctl command encoding: 32 bits total, command in lower 16 bits,
         * size of the parameter structure in the lower 14 bits of the
         * upper 16 bits.
         * Encoding the size of the parameter structure in the ioctl request
         * is useful for catching programs compiled with old versions
         * and to avoid overwriting user space outside the user buffer area.
         * The highest 2 bits are reserved for indicating the ``access mode''.
         * NOTE: This limits the max parameter size to 16kB -1 !
         */

        /*
         * The following is for compatibility across the various Linux
         * platforms.  The generic ioctl numbering scheme doesn't really enforce
         * a type field.  De facto, however, the top 8 bits of the lower 16
         * bits are indeed used as a type field, so we might just as well make
         * this explicit here.  Please be sure to use the decoding macros
         * below from now on.
         */
        public const int _IOC_NRBITS = 8;

        public const int _IOC_TYPEBITS = 8;

        public const int _IOC_SIZEBITS = 14;

        public const int _IOC_DIRBITS = 2;

        public const int _IOC_NRMASK = ((1 << _IOC_NRBITS) - 1);

        public const int _IOC_TYPEMASK = ((1 << _IOC_TYPEBITS) - 1);
        public const int _IOC_SIZEMASK = ((1 << _IOC_SIZEBITS) - 1);
        public const int _IOC_DIRMASK = ((1 << _IOC_DIRBITS) - 1);

        public const int _IOC_NRSHIFT = 0;
        public const int _IOC_TYPESHIFT = (_IOC_NRSHIFT + _IOC_NRBITS);
        public const int _IOC_SIZESHIFT = (_IOC_TYPESHIFT + _IOC_TYPEBITS);
        public const int _IOC_DIRSHIFT = (_IOC_SIZESHIFT + _IOC_SIZEBITS);

        /*
         * Direction bits, which any architecture can choose to override
         * before including this file.
         *
         * NOTE: _IOC_WRITE means userland is writing and kernel is
         * reading. _IOC_READ means userland is reading and kernel is writing.
         */
        public const int _IOC_NONE = 0;
        public const int _IOC_WRITE = 1;
        public const int _IOC_READ = 2;

        public static int _IOC(int dir, int type, int nr, int size)
        {
            return (((dir) << _IOC_DIRSHIFT) | ((type) << _IOC_TYPESHIFT) | ((nr) << _IOC_NRSHIFT) | ((size) << _IOC_SIZESHIFT));
        }

        public static int _IOC_TYPECHECK(object t) => Marshal.SizeOf(t);

        public static int _IOC_TYPECHECK(Type t) => Marshal.SizeOf(t);

        public static int _IOC_TYPECHECK<T>() => Marshal.SizeOf<T>();

        /*
         * Used to create numbers.
         *
         * NOTE: _IOW means userland is writing and kernel is reading. _IOR
         * means userland is reading and kernel is writing.
         */

        public static int _IO(int type, int nr) => _IOC(_IOC_NONE, (type), (nr), 0);

        public static int _IOR<T>(int type, int nr) => _IOC(_IOC_READ, (type), (nr), (_IOC_TYPECHECK<T>()));

        public static int _IOW<T>(int type, int nr) => _IOC(_IOC_WRITE, (type), (nr), (_IOC_TYPECHECK<T>()));

        public static int _IOWR<T>(int type, int nr) => _IOC(_IOC_READ | _IOC_WRITE, (type), (nr), (_IOC_TYPECHECK<T>()));

        /* used to decode ioctl numbers.. */

        public static int _IOC_DIR(int nr) => (((nr) >> _IOC_DIRSHIFT) & _IOC_DIRMASK);

        public static int _IOC_TYPE(int nr) => (((nr) >> _IOC_TYPESHIFT) & _IOC_TYPEMASK);

        public static int _IOC_NR(int nr) => (((nr) >> _IOC_NRSHIFT) & _IOC_NRMASK);

        public static int _IOC_SIZE(int nr) => (((nr) >> _IOC_SIZESHIFT) & _IOC_SIZEMASK);

        /* ...and for the drivers/sound files... */
        public const int IOC_IN = (_IOC_WRITE << _IOC_DIRSHIFT);
        public const int IOC_OUT = (_IOC_READ << _IOC_DIRSHIFT);
        public const int IOC_INOUT = ((_IOC_WRITE | _IOC_READ) << _IOC_DIRSHIFT);
        public const int IOCSIZE_MASK = (_IOC_SIZEMASK << _IOC_SIZESHIFT);
        public const int IOCSIZE_SHIFT = (_IOC_SIZESHIFT);
    }
}
