using System.Buffers.Binary;

namespace ModularPipelines.Caching;

internal static class ZipCentralDirectory
{
    private const uint EndOfCentralDirectorySignature = 0x06054B50;
    private const uint Zip64EndOfCentralDirectorySignature = 0x06064B50;
    private const uint Zip64EndOfCentralDirectoryLocatorSignature = 0x07064B50;
    private const int EndOfCentralDirectorySize = 22;
    private const int Zip64EndOfCentralDirectorySize = 56;
    private const int Zip64EndOfCentralDirectoryLocatorSize = 20;

    public static long ReadEntryCount(string path)
    {
        using var stream = new FileStream(
            path,
            FileMode.Open,
            FileAccess.Read,
            FileShare.Read,
            64 * 1024,
            FileOptions.SequentialScan);
        var tailLength = checked((int) Math.Min(
            stream.Length,
            EndOfCentralDirectorySize + ushort.MaxValue));
        if (tailLength < EndOfCentralDirectorySize)
        {
            throw new InvalidDataException("Module cache entry is not a valid ZIP archive.");
        }

        var tail = new byte[tailLength];
        stream.Seek(-tailLength, SeekOrigin.End);
        stream.ReadExactly(tail);
        for (var offset = tail.Length - EndOfCentralDirectorySize; offset >= 0; offset--)
        {
            var record = tail.AsSpan(offset);
            if (BinaryPrimitives.ReadUInt32LittleEndian(record) != EndOfCentralDirectorySignature
                || offset + EndOfCentralDirectorySize
                + BinaryPrimitives.ReadUInt16LittleEndian(record[20..]) != tail.Length)
            {
                continue;
            }

            ValidateSingleDiskArchive(record);
            var entryCount = BinaryPrimitives.ReadUInt16LittleEndian(record[10..]);
            return entryCount == ushort.MaxValue
                ? ReadZip64EntryCount(stream, stream.Length - tailLength + offset)
                : entryCount;
        }

        throw new InvalidDataException("Module cache entry has no ZIP central directory.");
    }

    private static long ReadZip64EntryCount(Stream stream, long endOfCentralDirectoryOffset)
    {
        var locatorOffset = endOfCentralDirectoryOffset - Zip64EndOfCentralDirectoryLocatorSize;
        if (locatorOffset < 0)
        {
            throw new InvalidDataException("Module cache ZIP64 locator is missing.");
        }

        Span<byte> locator = stackalloc byte[Zip64EndOfCentralDirectoryLocatorSize];
        stream.Position = locatorOffset;
        stream.ReadExactly(locator);
        if (BinaryPrimitives.ReadUInt32LittleEndian(locator)
            != Zip64EndOfCentralDirectoryLocatorSignature
            || BinaryPrimitives.ReadUInt32LittleEndian(locator[4..]) != 0
            || BinaryPrimitives.ReadUInt32LittleEndian(locator[16..]) != 1)
        {
            throw new InvalidDataException("Module cache ZIP64 locator is invalid.");
        }

        var recordOffset = BinaryPrimitives.ReadUInt64LittleEndian(locator[8..]);
        if (recordOffset > (ulong)(stream.Length - Zip64EndOfCentralDirectorySize))
        {
            throw new InvalidDataException("Module cache ZIP64 central directory is invalid.");
        }

        Span<byte> record = stackalloc byte[Zip64EndOfCentralDirectorySize];
        stream.Position = checked((long) recordOffset);
        stream.ReadExactly(record);
        var entriesOnDisk = BinaryPrimitives.ReadUInt64LittleEndian(record[24..]);
        var entryCount = BinaryPrimitives.ReadUInt64LittleEndian(record[32..]);
        if (BinaryPrimitives.ReadUInt32LittleEndian(record)
            != Zip64EndOfCentralDirectorySignature
            || BinaryPrimitives.ReadUInt64LittleEndian(record[4..]) < 44
            || BinaryPrimitives.ReadUInt32LittleEndian(record[16..]) != 0
            || BinaryPrimitives.ReadUInt32LittleEndian(record[20..]) != 0
            || entriesOnDisk != entryCount
            || entryCount > long.MaxValue)
        {
            throw new InvalidDataException("Module cache ZIP64 central directory is invalid.");
        }

        return (long) entryCount;
    }

    private static void ValidateSingleDiskArchive(ReadOnlySpan<byte> record)
    {
        if (BinaryPrimitives.ReadUInt16LittleEndian(record[4..]) != 0
            || BinaryPrimitives.ReadUInt16LittleEndian(record[6..]) != 0
            || BinaryPrimitives.ReadUInt16LittleEndian(record[8..])
            != BinaryPrimitives.ReadUInt16LittleEndian(record[10..]))
        {
            throw new InvalidDataException("Multi-disk module cache ZIP archives are not supported.");
        }
    }
}
