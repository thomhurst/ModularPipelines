using System.Text;
using System.Text.RegularExpressions;

namespace ModularPipelines.Caching;

internal static class ModuleCacheFileResolver
{
    public static IReadOnlyList<string> ResolveFiles(
        string workingDirectory,
        IEnumerable<string> patterns,
        int maximumFiles,
        string? excludedDirectory = null)
    {
        if (maximumFiles <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(maximumFiles), "The maximum input file count must be positive.");
        }

        var root = Path.GetFullPath(workingDirectory);
        var excludedRoot = excludedDirectory is null ? null : Path.GetFullPath(excludedDirectory);
        var files = new HashSet<string>(PathComparer);
        var globs = new List<Regex>();

        foreach (var rawPattern in patterns)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(rawPattern);
            var pattern = NormalizePattern(root, rawPattern);

            if (pattern.IndexOfAny(['*', '?']) >= 0)
            {
                globs.Add(CreateGlobRegex(pattern));
                continue;
            }

            var path = GetContainedPath(root, pattern);
            if (File.Exists(path))
            {
                AddFile(files, path, maximumFiles, excludedRoot);
            }
            else if (Directory.Exists(path))
            {
                foreach (var file in Directory.EnumerateFiles(path, "*", SearchOption.AllDirectories))
                {
                    AddFile(files, file, maximumFiles, excludedRoot);
                }
            }
        }

        if (globs.Count > 0)
        {
            foreach (var file in Directory.EnumerateFiles(root, "*", SearchOption.AllDirectories))
            {
                var relativePath = NormalizeSeparators(Path.GetRelativePath(root, file));
                if (globs.Any(regex => regex.IsMatch(relativePath)))
                {
                    AddFile(files, file, maximumFiles, excludedRoot);
                }
            }
        }

        return files.Order(PathComparer).ToArray();
    }

    public static IReadOnlyList<string> ResolveDirectories(
        string workingDirectory,
        IEnumerable<string> patterns,
        int maximumDirectories,
        string? excludedDirectory = null)
    {
        if (maximumDirectories <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(maximumDirectories),
                "The maximum directory count must be positive.");
        }

        var root = Path.GetFullPath(workingDirectory);
        var excludedRoot = excludedDirectory is null ? null : Path.GetFullPath(excludedDirectory);
        var directories = new HashSet<string>(PathComparer);
        var globs = new List<Regex>();

        foreach (var rawPattern in patterns)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(rawPattern);
            var pattern = NormalizePattern(root, rawPattern);

            if (pattern.IndexOfAny(['*', '?']) >= 0)
            {
                globs.Add(CreateGlobRegex(pattern));
                continue;
            }

            var path = GetContainedPath(root, pattern);
            if (!Directory.Exists(path))
            {
                continue;
            }

            AddDirectory(directories, path, maximumDirectories, excludedRoot);
            foreach (var directory in Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories))
            {
                AddDirectory(directories, directory, maximumDirectories, excludedRoot);
            }
        }

        if (globs.Count > 0)
        {
            foreach (var directory in Directory.EnumerateDirectories(root, "*", SearchOption.AllDirectories))
            {
                var relativePath = NormalizeSeparators(Path.GetRelativePath(root, directory));
                if (globs.Any(regex => regex.IsMatch(relativePath)))
                {
                    AddDirectory(directories, directory, maximumDirectories, excludedRoot);
                }
            }
        }

        return directories.Order(PathComparer).ToArray();
    }

    public static string GetRelativePath(string workingDirectory, string path)
    {
        var root = Path.GetFullPath(workingDirectory);
        var fullPath = Path.GetFullPath(path);
        EnsureContained(root, fullPath);
        return NormalizeSeparators(Path.GetRelativePath(root, fullPath));
    }

    private static string NormalizePattern(string root, string pattern)
    {
        var normalized = NormalizeSeparators(pattern.Trim());
        if (Path.IsPathRooted(pattern))
        {
            var wildcardIndex = normalized.IndexOfAny(['*', '?']);
            var nonWildcardPrefix = wildcardIndex < 0 ? normalized : normalized[..wildcardIndex];
            EnsureContained(root, Path.GetFullPath(nonWildcardPrefix));
            normalized = NormalizeSeparators(Path.GetRelativePath(root, pattern));
        }

        if (normalized == ".." || normalized.StartsWith("../", StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                $"Cache path pattern '{pattern}' escapes working directory '{root}'.");
        }

        return normalized.TrimStart('/');
    }

    private static string GetContainedPath(string root, string relativePath)
    {
        var fullPath = Path.GetFullPath(Path.Combine(root, relativePath));
        EnsureContained(root, fullPath);
        return fullPath;
    }

    private static void EnsureContained(string root, string path)
    {
        var relative = Path.GetRelativePath(root, path);
        if (relative == ".."
            || relative.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
            || Path.IsPathRooted(relative))
        {
            throw new InvalidOperationException($"Path '{path}' escapes working directory '{root}'.");
        }
    }

    private static void AddFile(
        HashSet<string> files,
        string path,
        int maximumFiles,
        string? excludedRoot)
    {
        var fullPath = Path.GetFullPath(path);
        if (excludedRoot is not null && IsWithin(excludedRoot, fullPath))
        {
            return;
        }

        files.Add(fullPath);
        if (files.Count > maximumFiles)
        {
            throw new InvalidOperationException(
                $"Cache input expansion exceeded the configured limit of {maximumFiles:N0} files. "
                + "Narrow the input globs or increase ModuleCacheOptions.MaximumInputFiles.");
        }
    }

    private static void AddDirectory(
        HashSet<string> directories,
        string path,
        int maximumDirectories,
        string? excludedRoot)
    {
        var fullPath = Path.GetFullPath(path);
        if (excludedRoot is not null && IsWithin(excludedRoot, fullPath))
        {
            return;
        }

        directories.Add(fullPath);
        if (directories.Count > maximumDirectories)
        {
            throw new InvalidOperationException(
                $"Cache artifact expansion exceeded the configured limit of {maximumDirectories:N0} directories.");
        }
    }

    private static bool IsWithin(string root, string path)
    {
        var relative = Path.GetRelativePath(root, path);
        return relative == "."
               || (!relative.StartsWith($"..{Path.DirectorySeparatorChar}", StringComparison.Ordinal)
                   && relative != ".."
                   && !Path.IsPathRooted(relative));
    }

    private static Regex CreateGlobRegex(string glob)
    {
        var expression = new StringBuilder("^");
        for (var index = 0; index < glob.Length; index++)
        {
            switch (glob[index])
            {
                case '*':
                    if (index + 1 < glob.Length && glob[index + 1] == '*')
                    {
                        index++;
                        if (index + 1 < glob.Length && glob[index + 1] == '/')
                        {
                            index++;
                            expression.Append("(?:.*/)?");
                        }
                        else
                        {
                            expression.Append(".*");
                        }
                    }
                    else
                    {
                        expression.Append("[^/]*");
                    }

                    break;
                case '?':
                    expression.Append("[^/]");
                    break;
                default:
                    expression.Append(Regex.Escape(glob[index].ToString()));
                    break;
            }
        }

        expression.Append('$');
        return new Regex(
            expression.ToString(),
            RegexOptions.CultureInvariant | RegexOptions.Compiled,
            TimeSpan.FromSeconds(1));
    }

    private static string NormalizeSeparators(string path) => path.Replace('\\', '/');

    private static StringComparer PathComparer =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
}
