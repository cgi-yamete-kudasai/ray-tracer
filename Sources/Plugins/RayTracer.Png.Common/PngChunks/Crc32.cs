namespace RayTracer.Png.Common.PngChunks;

public static class Crc32
{
    private const uint Polynomial = 0xEDB88320;

    private static readonly uint[] Lookup;

    static Crc32()
    {
        Lookup = new uint[256];
        for (uint i = 0; i < 256; i++)
        {
            var value = i;
            for (var j = 0; j < 8; ++j)
            {
                if ((value & 1) != 0)
                {
                    value = (value >> 1) ^ Polynomial;
                }
                else
                {
                    value >>= 1;
                }
            }

            Lookup[i] = value;
        }
    }

    /// <summary>
    /// Calculate the CRC32 for data.
    /// </summary>
    public static uint Calculate(byte[] data)
    {
        var crc32 = uint.MaxValue;
        for (var i = 0; i < data.Length; i++)
        {
            var index = (crc32 ^ data[i]) & 0xFF;
            crc32 = (crc32 >> 8) ^ Lookup[index];
        }

        return crc32 ^ uint.MaxValue;
    }
}