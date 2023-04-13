using System.Runtime.InteropServices;

namespace RayTracer.Library.Memory;

[StructLayout(LayoutKind.Sequential)]
public readonly struct BigEndianInt
{
    public readonly byte Byte1;

    public readonly byte Byte2;

    public readonly byte Byte3;

    public readonly byte Byte4;

    public BigEndianInt(uint i)
    {
        Byte1 = (byte)((i & 0xFF000000) >> 24);
        Byte2 = (byte)((i & 0x00FF0000) >> 16);
        Byte3 = (byte)((i & 0x0000FF00) >> 8);
        Byte4 = (byte)((i & 0x000000FF) >> 0);
    }

    public static implicit operator BigEndianInt(uint i) => new(i);

    public static implicit operator uint(BigEndianInt i)
    {
        return (uint)(i.Byte1 << 24 | i.Byte2 << 16 | i.Byte3 << 8 | i.Byte4);
    }
}
