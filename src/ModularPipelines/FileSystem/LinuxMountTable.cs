namespace ModularPipelines.FileSystem;

/// <summary>Compares paths in filesystem coordinates, including Linux bind mounts.</summary>
internal sealed class LinuxMountTable(IEnumerable<string> lines)
{
    // mountinfo fields 3, 4 and 5 identify the device, filesystem root and mount point.
    // https://man7.org/linux/man-pages/man5/proc_pid_mountinfo.5.html
    private readonly Mount[] _mounts = lines.Select(Parse).ToArray();

    internal void ValidateCopyTarget(string sourcePath, string targetPath)
    {
        var source = Locate(sourcePath);
        var sources = new List<Location> { source };
        foreach (var mount in _mounts)
        {
            if (Contains(sourcePath, mount.Point) && FindMount(mount.Point) == mount)
            {
                sources.Add(new Location(mount.Device, mount.Root));
            }
        }

        RejectOverlap(Locate(targetPath));
        // Existing mounts below the destination can redirect later writes into the source too.
        foreach (var mount in _mounts)
        {
            if (Contains(targetPath, mount.Point) && FindMount(mount.Point) == mount)
            {
                RejectOverlap(new Location(mount.Device, mount.Root), nestedMount: true);
            }
        }

        void RejectOverlap(Location target, bool nestedMount = false)
        {
            if (sources.Any(location => location.Device == target.Device
                && (Contains(location.Path, target.Path) || (nestedMount && Contains(target.Path, location.Path)))))
            {
                throw new ArgumentException("The destination must not be the source folder or one of its descendants.", nameof(targetPath));
            }
        }
    }

    private Location Locate(string path)
    {
        var mount = FindMount(path);
        var suffix = path[mount.Point.Length..].TrimStart('/');
        var filesystemPath = suffix.Length == 0 ? mount.Root : $"{mount.Root.TrimEnd('/')}/{suffix}";
        return new Location(mount.Device, filesystemPath);
    }

    private Mount FindMount(string path)
    {
        // Follow the visible mount tree instead of selecting a hidden mount by path length.
        var current = _mounts.SingleOrDefault(mount => mount.Point == "/"
            && (mount.Id == mount.ParentId || !_mounts.Any(parent => parent.Id == mount.ParentId)))
            ?? throw new IOException("Missing root mount while validating the copy destination.");
        for (var depth = 0; depth < _mounts.Length; depth++)
        {
            var child = _mounts
                .Where(mount => mount.ParentId == current.Id && mount.Id != current.Id && Contains(mount.Point, path))
                .OrderBy(mount => mount.Point.Length)
                .FirstOrDefault();
            if (child is null)
            {
                return current;
            }

            current = child;
        }

        throw new IOException("Invalid Linux mount table while validating the copy destination.");
    }

    private static bool Contains(string parent, string path) => path == parent
        || (path.StartsWith(parent, StringComparison.Ordinal) && (parent == "/" || path[parent.Length] == '/'));

    private static Mount Parse(string line)
    {
        var fields = line.Split(' ');
        if (fields.Length < 10)
        {
            throw new IOException("Invalid Linux mount table while validating the copy destination.");
        }

        return new Mount(fields[0], fields[1], fields[2], Unescape(fields[3]), Unescape(fields[4]));
    }

    private static string Unescape(string path) => path
        .Replace("\\040", " ", StringComparison.Ordinal)
        .Replace("\\011", "\t", StringComparison.Ordinal)
        .Replace("\\012", "\n", StringComparison.Ordinal)
        .Replace("\\134", "\\", StringComparison.Ordinal);

    private sealed record Mount(string Id, string ParentId, string Device, string Root, string Point);

    private sealed record Location(string Device, string Path);
}
