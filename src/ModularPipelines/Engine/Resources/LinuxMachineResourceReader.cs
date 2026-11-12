using System.Globalization;

namespace ModularPipelines.Engine.Resources;

internal sealed class LinuxMachineResourceReader : IMachineResourceReader
{
    public bool TryRead(out MachineResourceReading reading)
    {
        reading = default;
        if (!TryParseCpu(File.ReadLines("/proc/stat").FirstOrDefault(), out var idle, out var total)
            || !TryParseMemory(File.ReadAllLines("/proc/meminfo"), out var memoryTotal, out var memoryAvailable))
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

    internal static bool TryParseMemory(IReadOnlyList<string> lines, out long totalBytes, out long availableBytes)
    {
        totalBytes = FindKilobytes(lines, "MemTotal:") * 1024;
        availableBytes = FindKilobytes(lines, "MemAvailable:") * 1024;
        return totalBytes > 0 && availableBytes >= 0;
    }

    private static long FindKilobytes(IReadOnlyList<string> lines, string name)
    {
        var line = lines.FirstOrDefault(candidate => candidate.StartsWith(name, StringComparison.Ordinal));
        var fields = line?.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (fields is { Length: >= 2 }
            && long.TryParse(fields[1], NumberStyles.None, CultureInfo.InvariantCulture, out var kilobytes))
        {
            return kilobytes;
        }

        return -1;
    }
}
