using RayTracer.Imaging;
using RayTracer.Imaging.IO.Writers;
using RayTracer.Imaging.Png;
using RayTracer.Imaging.Png.PngChunks;
using RayTracer.Library.Extensions;
using RayTracer.Library.Memory;
using RayTracer.Library.Utils;

namespace RayTracer.Png.Writer;

public class PngBitmapWriter : IBitmapWriter
{
    public string Format => "png";

    public void Write(Stream destination, Bitmap bitmap)
    {
        destination.Write(FileSignatures.Png);
        
        PngChunk ihdrChunk = CreateIHDRChunk(bitmap);
        WriteChunk(destination, ihdrChunk);

        PngChunk idatChunk = CreateIDATChunk(bitmap);
        WriteChunk(destination, idatChunk);

        PngChunk iendChunk = CreateIENDChunk();
        WriteChunk(destination, iendChunk);
    }

    private PngChunk CreateIHDRChunk(Bitmap bitmap)
    {
        PngHeader header = new(bitmap.Width, bitmap.Height);
        MemoryStream dataStream = new MemoryStream(new byte[13]);

        dataStream.NativeWrite(new BigEndianInt(header.Width));
        dataStream.NativeWrite(new BigEndianInt(header.Height));
        dataStream.WriteByte(header.BitDepth);
        dataStream.WriteByte((byte)header.PngColorType);
        dataStream.WriteByte(header.CompressionMethod);
        dataStream.WriteByte(header.FilterMethod);
        dataStream.WriteByte((byte)header.InterlaceMethod);

        return new(13, PngChunkType.IHDR.ToArray(), dataStream.ToArray());
    }

    private PngChunk CreateIENDChunk()
    {
        return new(0, PngChunkType.IEND.ToArray(), Array.Empty<byte>());
    }

    private PngChunk CreateIDATChunk(Bitmap bitmap)
    {
        MemoryStream dataStream = new MemoryStream();
        dataStream.WriteByte(0x08);
        dataStream.WriteByte(0x1D);
        PngEncoder.EncodeImageData(bitmap, dataStream);

        return new((uint)dataStream.ToArray().Length, PngChunkType.IDAT.ToArray(), dataStream.ToArray());
    }

    private void WriteChunk(Stream stream, PngChunk chunk)
    {
        stream.NativeWrite(new BigEndianInt(chunk.DataLength));
        stream.Write(chunk.ChunkType);
        stream.Write(chunk.Data);
        stream.NativeWrite(new BigEndianInt(chunk.Crc));
    }
}