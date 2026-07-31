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
        return ResolvePaths(
            workingDirectory,
            patterns,
            maximumFiles,
            nameof(maximumFiles),
            "The maximum input file count must be positive.",
            excludedDirectory,
            ResolveExactFilePattern,
            static root => EnumerateWithoutFollowingDirectoryLinks(root, includeDirectories: false),
            maximum => $"Cache input expansion exceeded the configured limit of {maximum:N0} files. "
                       + "Narrow the input globs or increase ModuleCacheOptions.MaximumInputFiles.");
    }

    public static IReadOnlyList<string> ResolveDirectories(
        string workingDirectory,
        IEnumerable<string> patterns,
        int maximumDirectories,
        string? excludedDirectory = null)
    {
        return ResolvePaths(
            workingDirectory,
            patterns,
            maximumDirectories,
            nameof(maximumDirectories),
            "The maximum directory count must be positive.",
            excludedDirectory,
            ResolveExactDirectoryPattern,
            static root => EnumerateWithoutFollowingDirectoryLinks(root, includeDirectories: true),
            maximum => $"Cache artifact expansion exceeded the configured limit of {maximum:N0} directories.");
    }

    public static IReadOnlyList<string> ResolveDirectoryLinks(
        string workingDirectory,
        IEnumerable<string> patterns,
        int maximumLinks,
        string? excludedDirectory = null)
    {
        return ResolvePaths(
            workingDirectory,
            patterns,
            maximumLinks,
            nameof(maximumLinks),
            "The maximum directory link count must be positive.",
            excludedDirectory,
            ResolveExactDirectoryLinkPattern,
            EnumerateDirectoryLinksWithoutFollowing,
            maximum => $"Cache artifact expansion exceeded the configured limit of {maximum:N0} directory links.",
            allowFinalLinkedComponent: true);
    }

    public static string GetRelativePath(string workingDirectory, string path)
    {
        var root = Path.GetFullPath(workingDirectory);
        var fullPath = Path.GetFullPath(path);
        EnsureContained(root, fullPath);
        return NormalizeSeparators(Path.GetRelativePath(root, fullPath));
    }

    public static bool IsWithinDeclaredArtifactScope(
        string workingDirectory,
        string path,
        IEnumerable<string> patterns)
    {
        var root = Path.GetFullPath(workingDirectory);
        var fullPath = Path.GetFullPath(path);
        EnsureContained(root, fullPath);
        var relativePath = NormalizeSeparators(Path.GetRelativePath(root, fullPath));

        foreach (var rawPattern in patterns)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(rawPattern);
            var pattern = NormalizePattern(root, rawPattern);
            if (pattern.IndexOfAny(['*', '?']) >= 0)
            {
                if (CreateGlobRegex(pattern).IsMatch(relativePath))
                {
                    return true;
                }

                continue;
            }

            var declaredRoot = GetContainedPath(root, pattern);
            if (IsWithin(declaredRoot, fullPath))
            {
                return true;
            }
        }

        return false;
    }

    private static string[] ResolvePaths(
        string workingDirectory,
        IEnumerable<string> patterns,
        int maximumPaths,
        string maximumPathsParameterName,
        string invalidMaximumMessage,
        string? excludedDirectory,
        Func<string, IEnumerable<string>> resolveExactPattern,
        Func<string, IEnumerable<string>> enumeratePaths,
        Func<int, string> expansionLimitMessage,
        bool allowFinalLinkedComponent = false)
    {
        if (maximumPaths <= 0)
        {
            throw new ArgumentOutOfRangeException(maximumPathsParameterName, invalidMaximumMessage);
        }

        var root = Path.GetFullPath(workingDirectory);
        var excludedRoot = excludedDirectory is null
            ? null
            : Path.GetFullPath(excludedDirectory);
        if (excludedRoot is not null && !IsWithin(root, excludedRoot))
        {
            excludedRoot = null;
        }

        var paths = new HashSet<string>(PathComparer);
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
            if (HasLinkedDirectoryComponent(root, path, allowFinalLinkedComponent))
            {
                continue;
            }

            AddPaths(
                paths,
                resolveExactPattern(path),
                maximumPaths,
                excludedRoot,
                expansionLimitMessage);
        }

        if (globs.Count > 0)
        {
            foreach (var path in enumeratePaths(root))
            {
                var relativePath = NormalizeSeparators(Path.GetRelativePath(root, path));
                if (globs.Any(regex => regex.IsMatch(relativePath)))
                {
                    AddPath(paths, path, maximumPaths, excludedRoot, expansionLimitMessage);
                }
            }
        }

        return [.. paths.Order(PathComparer)];
    }

    private static IEnumerable<string> ResolveExactFilePattern(string path)
    {
        if (File.Exists(path))
        {
            return [path];
        }

        return Directory.Exists(path)
            ? EnumerateWithoutFollowingDirectoryLinks(path, includeDirectories: false)
            : [];
    }

    private static IEnumerable<string> ResolveExactDirectoryPattern(string path)
    {
        return Directory.Exists(path)
            ? EnumerateDirectoryAndDescendants(path)
            : [];
    }

    private static IEnumerable<string> ResolveExactDirectoryLinkPattern(string path)
    {
        if (!Directory.Exists(path))
        {
            return [];
        }

        return IsDirectoryLink(path)
            ? [path]
            : EnumerateDirectoryLinksWithoutFollowing(path);
    }

    private static IEnumerable<string> EnumerateDirectoryAndDescendants(string root)
    {
        yield return root;

        foreach (var directory in EnumerateWithoutFollowingDirectoryLinks(root, includeDirectories: true))
        {
            yield return directory;
        }
    }

    private static IEnumerable<string> EnumerateWithoutFollowingDirectoryLinks(
        string root,
        bool includeDirectories)
    {
        var pendingDirectories = new Stack<string>();
        pendingDirectories.Push(root);

        while (pendingDirectories.TryPop(out var directory))
        {
            foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
            {
                var attributes = File.GetAttributes(entry);
                var isDirectory = (attributes & FileAttributes.Directory) != 0;
                if (!isDirectory)
                {
                    if (!includeDirectories)
                    {
                        yield return entry;
                    }

                    continue;
                }

                if ((attributes & FileAttributes.ReparsePoint) != 0)
                {
                    continue;
                }

                if (includeDirectories)
                {
                    yield return entry;
                }

                pendingDirectories.Push(entry);
            }
        }
    }

    private static IEnumerable<string> EnumerateDirectoryLinksWithoutFollowing(string root)
    {
        var pendingDirectories = new Stack<string>();
        pendingDirectories.Push(root);

        while (pendingDirectories.TryPop(out var directory))
        {
            foreach (var entry in Directory.EnumerateFileSystemEntries(directory))
            {
                var attributes = File.GetAttributes(entry);
                if ((attributes & FileAttributes.Directory) == 0)
                {
                    continue;
                }

                if ((attributes & FileAttributes.ReparsePoint) != 0)
                {
                    yield return entry;
                    continue;
                }

                pendingDirectories.Push(entry);
            }
        }
    }

    private static bool IsDirectoryLink(string path)
    {
        var attributes = File.GetAttributes(path);
        return (attributes & (FileAttributes.Directory | FileAttributes.ReparsePoint))
               == (FileAttributes.Directory | FileAttributes.ReparsePoint);
    }

    private static bool HasLinkedDirectoryComponent(
        string root,
        string path,
        bool allowFinalLinkedComponent)
    {
        var relativePath = Path.GetRelativePath(root, path);
        var currentPath = root;
        var components = relativePath.Split(
            [Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar],
            StringSplitOptions.RemoveEmptyEntries);

        for (var index = 0; index < components.Length; index++)
        {
            currentPath = Path.Combine(currentPath, components[index]);
            if (Directory.Exists(currentPath)
                && IsDirectoryLink(currentPath)
                && (!allowFinalLinkedComponent || index < components.Length - 1))
            {
                return true;
            }
        }

        return false;
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

    private static void AddPaths(
        HashSet<string> paths,
        IEnumerable<string> candidates,
        int maximumPaths,
        string? excludedRoot,
        Func<int, string> expansionLimitMessage)
    {
        foreach (var candidate in candidates)
        {
            AddPath(paths, candidate, maximumPaths, excludedRoot, expansionLimitMessage);
        }
    }

    private static void AddPath(
        HashSet<string> paths,
        string path,
        int maximumPaths,
        string? excludedRoot,
        Func<int, string> expansionLimitMessage)
    {
        var fullPath = Path.GetFullPath(path);
        if (excludedRoot is not null && IsWithin(excludedRoot, fullPath))
        {
            return;
        }

        paths.Add(fullPath);
        if (paths.Count > maximumPaths)
        {
            throw new InvalidOperationException(expansionLimitMessage(maximumPaths));
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
        var options = RegexOptions.CultureInvariant | RegexOptions.Compiled;
        if (OperatingSystem.IsWindows())
        {
            options |= RegexOptions.IgnoreCase;
        }

        return new Regex(
            expression.ToString(),
            options,
            TimeSpan.FromSeconds(1));
    }

    private static string NormalizeSeparators(string path) => path.Replace('\\', '/');

    private static StringComparer PathComparer =>
        OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal;
}
