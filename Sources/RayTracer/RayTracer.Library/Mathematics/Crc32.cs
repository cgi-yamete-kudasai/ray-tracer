using System;

namespace RayTracer.Library.Mathematics;

public struct Crc32
{
    private static readonly uint[] CrcTable = new uint[256];

    private uint _crc;

    static Crc32()
    {
        InitializeLookupTable();
    }

    public Crc32()
    {
        _crc = 0xFFFFFFFF;
    }
    
    public uint Get()
    {
        return _crc ^ 0xFFFFFFFF;
    }

    public void Append(ReadOnlySpan<byte> data)
    {
        foreach (byte b in data)
            _crc = CrcTable[(_crc ^ b) & 0xFF] ^ (_crc >> 8);
    }

    private static void InitializeLookupTable()
    {
        for (uint i = 0; i < 256; i++)
        {
            uint c = i;

            for (int j = 0; j < 8; j++)
            {
                if ((c & 1) == 1)
                {
                    c = 0xEDB88320 ^ (c >> 1);
                }
                else
                {
                    c = c >> 1;
                }
            }

            CrcTable[i] = c;
        }
    }
}
