using RayTracer.Library.Utils;

namespace RayTracer.Png.Common.Filters;

public static class PngFilterProcessor
{
    public static void Process(Span<byte> previousRow, Span<byte> currentRow, FilterType filterType,
        Stream outputStream, FilterMode filterMode, int bytesPerPixel = 3)
    {
        if (filterMode == FilterMode.Apply)
        {
            ApplyFilter(previousRow, currentRow, filterType, outputStream, bytesPerPixel);
        }
        else
        {
            ReverseFilter(previousRow, currentRow, filterType, outputStream, bytesPerPixel);
        }
    }

    private static void ApplyFilter(Span<byte> previousRaw, Span<byte> currentRaw, FilterType filter,
        Stream stream, int bytesPerPixel = 3)
    {
        for (int j = 0; j < currentRaw.Length; j++)
        {
            byte value = GetFilteredValue(j, previousRaw, currentRaw, filter, FilterMode.Apply, bytesPerPixel);
            stream.WriteByte(value);
        }
    }

    private static void ReverseFilter(Span<byte> previousRaw, Span<byte> currentRaw, FilterType filterType,
        Stream stream, int bytesPerPixel = 3)
    {
        stream.WriteByte((byte)filterType);
        for (int i = 0; i < currentRaw.Length; i++)
        {
            byte value = GetFilteredValue(i, previousRaw, currentRaw, filterType, FilterMode.Reverse, bytesPerPixel);
            currentRaw[i] = value;
            stream.WriteByte(value);
        }
    }

    public static FilterType FindBestFilter(Bitmap bitmap, Span<byte> previousRaw, Span<byte> currentRaw)
    {
        int width = bitmap.Width;

        FilterType bestFilter = 0;
        int bestFilterValue = int.MaxValue;

        for (int i = 0; i < 5; i++)
        {
            int filterValue = 0;
            FilterType currentFilter = (FilterType)i;
            for (int j = 0; j < width * 3; j++)
            {
                int value = GetFilteredValue(j, previousRaw, currentRaw, currentFilter);
                filterValue += value * value;
            }

            if (filterValue < bestFilterValue)
            {
                bestFilter = currentFilter;
                bestFilterValue = filterValue;
            }
        }

        return bestFilter;
    }

    private static byte GetFilteredValue(int index, ReadOnlySpan<byte> previousRaw, ReadOnlySpan<byte> currentRaw,
        FilterType filterType, FilterMode filterMode = FilterMode.Apply, int bytesPerPixel = 3)
    {
        byte value = currentRaw[index];
        byte leftValue = index < bytesPerPixel ? (byte)0 : currentRaw[index - bytesPerPixel];
        byte aboveValue = previousRaw[index];
        byte aboveLeftValue = index < bytesPerPixel ? (byte)0 : previousRaw[index - bytesPerPixel];

        return GetFilteredValue(value, leftValue, aboveValue, aboveLeftValue, filterType, filterMode);
    }

    private static byte GetFilteredValue(byte value, byte leftValue, byte aboveValue, byte aboveLeftValue,
        FilterType filterType, FilterMode filterMode) =>
        filterType switch
        {
            FilterType.None => value,
            FilterType.Sub => filterMode == FilterMode.Reverse ? (byte)(value + leftValue) : (byte)(value - leftValue),
            FilterType.Up => filterMode == FilterMode.Reverse ? (byte)(value + aboveValue) : (byte)(value - aboveValue),
            FilterType.Average => filterMode == FilterMode.Reverse
                ? (byte)(value + (leftValue + aboveValue) / 2)
                : (byte)(value - (leftValue + aboveValue) / 2),
            FilterType.Paeth => filterMode == FilterMode.Reverse
                ? (byte)(value + CalculatePaethPredictor(leftValue, aboveValue, aboveLeftValue))
                : (byte)(value - CalculatePaethPredictor(leftValue, aboveValue, aboveLeftValue)),
            _ => throw new ArgumentOutOfRangeException(nameof(filterType), filterType, null)
        };

    private static byte CalculatePaethPredictor(byte a, byte b, byte c)
    {
        var p = a + b - c;
        var pa = Math.Abs(p - a);
        var pb = Math.Abs(p - b);
        var pc = Math.Abs(p - c);

        if (pa <= pb && pa <= pc)
        {
            return a;
        }

        if (pb <= pc)
        {
            return b;
        }

        return c;
    }
}