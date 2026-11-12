using System.Globalization;

namespace ModularPipelines.Engine.Resources;

internal sealed class LinuxMachineResourceReader : IMachineResourceReader
{
    public bool TryRead(out MachineResourceReading reading)
    {
        reading = default;
        if (!TryParseCpu(File.ReadLines("/proc/stat").FirstOrDefault(), out var idle, out var total)
            || !TryParseMemory(File.ReadLines("/proc/meminfo"), out var memoryTotal, out var memoryAvailable))
        {
            return false;
        }

        reading = new MachineResourceReading(idle, total, memoryTotal, memoryAvailable);
        return true;
    }

    /// <summary>
    /// Parses the aggregate <c>cpu</c> line of <c>/proc/stat</c>:
    /// user, nice, system, idle, iowait, irq, softirq, steal, then guest fields already counted in user and nice.
    /// </summary>
    internal static bool TryParseCpu(string? line, out long idle, out long total)
    {
        idle = 0;
        total = 0;
        if (line is null)
        {
            return false;
        }

        var fields = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (fields.Length < 5 || fields[0] != "cpu")
        {
            return false;
        }

        for (var index = 1; index < fields.Length && index <= 8; index++)
        {
            if (!long.TryParse(fields[index], NumberStyles.None, CultureInfo.InvariantCulture, out var value))
            {
                return false;
            }

            total += value;

            // Fields 4 and 5 are idle and iowait.
            if (index is 4 or 5)
            {
                idle += value;
            }
        }

        return true;
    }

    internal static bool TryParseMemory(IEnumerable<string> lines, out long totalBytes, out long availableBytes)
    {
        long? total = null;
        long? available = null;
        foreach (var line in lines)
        {
            if (line.StartsWith("MemTotal:", StringComparison.Ordinal))
            {
                total = ParseKilobytes(line);
            }
            else if (line.StartsWith("MemAvailable:", StringComparison.Ordinal))
            {
                available = ParseKilobytes(line);
            }

            if (total is not null && available is not null)
            {
                break;
            }
        }

        totalBytes = total ?? 0;
        availableBytes = available ?? 0;
        return total is not null && available is not null;
    }

    private static long? ParseKilobytes(string line)
    {
        var fields = line.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (fields.Length >= 2
            && long.TryParse(fields[1], NumberStyles.None, CultureInfo.InvariantCulture, out var kilobytes))
        {
            return kilobytes * 1024;
        }

        return null;
    }
}
