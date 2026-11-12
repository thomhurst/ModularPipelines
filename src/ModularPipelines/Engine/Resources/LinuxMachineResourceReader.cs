using System.Globalization;

namespace ModularPipelines.Engine.Resources;

internal sealed class LinuxMachineResourceReader : IMachineResourceReader
{
    private readonly Func<string, string?> _readFile;

    public LinuxMachineResourceReader() : this(File.ReadAllText)
    {
    }

    internal LinuxMachineResourceReader(Func<string, string?> readFile)
    {
        _readFile = readFile;
    }

    public bool TryRead(out MachineResourceReading reading)
    {
        reading = default;
        if (!TryParseCpu(Read("/proc/stat")?.Split('\n').FirstOrDefault(), out var idle, out var total)
            || !TryParseMemory(Lines(Read("/proc/meminfo")), out var memoryTotal, out var memoryAvailable))
        {
            return false;
        }

        var (directory, mount) = FindCgroup();
        (memoryTotal, memoryAvailable) = ReadCgroupMemory(directory, mount, memoryTotal, memoryAvailable);

        var cpuStat = directory is null ? null : Read(CombineLinuxPath(directory, "cpu.stat"));
        var cpuPressure = directory is null ? null : Read(CombineLinuxPath(directory, "cpu.pressure"));
        var memoryPressure = directory is null ? null : Read(CombineLinuxPath(directory, "memory.pressure"));
        var cpuPressureSource = cpuPressure is null ? "/proc/pressure/cpu" : directory;
        var memoryPressureSource = memoryPressure is null ? "/proc/pressure/memory" : directory;
        cpuPressure ??= Read("/proc/pressure/cpu");
        memoryPressure ??= Read("/proc/pressure/memory");
        var pressure = new ResourcePressureCounters(
            directory ?? string.Empty,
            FindCounter(cpuStat, "nr_throttled"),
            FindCounter(cpuStat, "throttled_usec"),
            FindPressureTotal(cpuPressure),
            FindPressureTotal(memoryPressure),
            cpuPressureSource,
            memoryPressureSource,
            ReadAncestorThrottling(directory, mount));
        reading = new MachineResourceReading(idle, total, memoryTotal, memoryAvailable, pressure);
        return true;
    }

    private IReadOnlyList<CgroupCpuThrottlingCounters> ReadAncestorThrottling(string? directory, string? mount)
    {
        List<CgroupCpuThrottlingCounters> counters = [];
        // cpu.stat bandwidth counters cover only the quota configured at that cgroup.
        for (var ancestor = directory; ancestor is not null && mount is not null && ancestor != mount;)
        {
            ancestor = GetLinuxParent(ancestor);
            if (ancestor is null)
            {
                break;
            }

            var stat = Read(CombineLinuxPath(ancestor, "cpu.stat"));
            counters.Add(new CgroupCpuThrottlingCounters(ancestor,
                FindCounter(stat, "nr_throttled"), FindCounter(stat, "throttled_usec")));
        }

        return counters;
    }

    private (long Total, long Available) ReadCgroupMemory(string? directory, string? mount, long hostTotal, long hostAvailable)
    {
        long total = hostTotal, available = hostAvailable;
        var found = false;
        // Compare each visible cgroup against its own limit: siblings can exhaust a larger parent.
        for (var ancestor = directory; ancestor is not null && mount is not null; ancestor = GetLinuxParent(ancestor))
        {
            if (TryNonNegative(Read(CombineLinuxPath(ancestor, "memory.max"))?.Trim(), out var limit)
                && limit > 0 && limit <= hostTotal
                && TryNonNegative(Read(CombineLinuxPath(ancestor, "memory.current"))?.Trim(), out var used))
            {
                var inactiveFile = FindCounter(Read(CombineLinuxPath(ancestor, "memory.stat")), "inactive_file") ?? 0;
                used -= Math.Min(used, inactiveFile);
                var headroom = Math.Clamp(limit - used, 0, limit);
                if (!found || (double) headroom / limit < (double) available / total)
                {
                    total = limit;
                    available = headroom;
                    found = true;
                }
            }

            if (ancestor == mount)
            {
                break;
            }
        }

        return (total, available);
    }

    // These are Linux virtual-filesystem paths, including when fixtures run on Windows.
    private static string CombineLinuxPath(string directory, string relative) =>
        relative.Length == 0 ? directory : $"{directory.TrimEnd('/')}/{relative}";

    private static string? GetLinuxParent(string directory)
    {
        var separator = directory.LastIndexOf('/');
        return directory == "/" || separator < 0 ? null : separator == 0 ? "/" : directory[..separator];
    }

    private string? Read(string path)
    {
        try
        {
            return _readFile(path);
        }
        catch (IOException)
        {
            return null;
        }
        catch (UnauthorizedAccessException)
        {
            return null;
        }
    }

    private (string? Directory, string? Mount) FindCgroup()
    {
        var membership = Lines(Read("/proc/self/cgroup"))
            .FirstOrDefault(line => line.StartsWith("0::", StringComparison.Ordinal))?[3..];
        if (membership is null || !membership.StartsWith('/') || membership.Split('/').Contains(".."))
        {
            return default;
        }

        foreach (var line in Lines(Read("/proc/self/mountinfo")))
        {
            var parts = line.Split(" - ", 2, StringSplitOptions.None);
            if (parts.Length != 2 || !parts[1].StartsWith("cgroup2 ", StringComparison.Ordinal))
            {
                continue;
            }

            var fields = parts[0].Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length < 5)
            {
                continue;
            }

            var root = UnescapeMount(fields[3]).TrimEnd('/');
            var mount = UnescapeMount(fields[4]);
            mount = mount.Length > 1 ? mount.TrimEnd('/') : mount;
            string relative;
            if (root.Length == 0)
            {
                relative = membership;
            }
            else if (membership == root || membership.StartsWith(root + "/", StringComparison.Ordinal))
            {
                relative = membership[root.Length..];
            }
            else if (membership == "/" && Lines(Read(CombineLinuxPath(mount, "cgroup.procs")))
                .Any(pid => TryNonNegative(pid, out var value) && value == Environment.ProcessId))
            {
                // Namespace-relative / alone cannot identify a bind-mounted subtree.
                // Accept it only when this process is listed in the mounted cgroup.
                relative = string.Empty;
            }
            else
            {
                continue;
            }

            if (mount.StartsWith('/'))
            {
                return (CombineLinuxPath(mount, relative.TrimStart('/')), mount);
            }
        }

        return default;
    }

    private static string UnescapeMount(string value) => value
        .Replace("\\040", " ", StringComparison.Ordinal)
        .Replace("\\011", "\t", StringComparison.Ordinal)
        .Replace("\\012", "\n", StringComparison.Ordinal)
        .Replace("\\134", "\\", StringComparison.Ordinal);

    private static string[] Lines(string? content) => content?.Split('\n', StringSplitOptions.RemoveEmptyEntries) ?? [];

    private static bool TryNonNegative(string? value, out long result) =>
        long.TryParse(value, NumberStyles.None, CultureInfo.InvariantCulture, out result) && result >= 0;

    private static long? FindCounter(string? content, string name)
    {
        foreach (var line in Lines(content))
        {
            var fields = line.Split((char[]?) null, StringSplitOptions.RemoveEmptyEntries);
            if (fields.Length == 2 && fields[0] == name && TryNonNegative(fields[1], out var value))
            {
                return value;
            }
        }

        return null;
    }

    private static long? FindPressureTotal(string? content)
    {
        var line = Lines(content).FirstOrDefault(candidate => candidate.StartsWith("some ", StringComparison.Ordinal));
        var field = line?.Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .FirstOrDefault(candidate => candidate.StartsWith("total=", StringComparison.Ordinal));
        return field is not null && TryNonNegative(field[6..], out var value) ? value : null;
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
