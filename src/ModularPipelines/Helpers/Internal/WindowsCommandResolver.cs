using System.Collections.Concurrent;

namespace ModularPipelines.Helpers.Internal;

internal static class WindowsCommandResolver
{
    private const string DefaultPathExtensions = ".COM;.EXE;.BAT;.CMD";

    private static readonly ConcurrentDictionary<ResolutionKey, string> ResolvedCommands = new();

    public static string? Resolve(
        string command,
        string? processDirectory = null,
        string? searchPath = null,
        string? pathExtensions = null,
        bool? isWindows = null)
    {
        if (!(isWindows ?? OperatingSystem.IsWindows()))
        {
            return null;
        }

        var baseDirectory = Path.GetFullPath(processDirectory ?? Environment.CurrentDirectory);
        var effectivePathExtensions =
            pathExtensions ?? Environment.GetEnvironmentVariable("PATHEXT") ?? DefaultPathExtensions;
        if (IsPath(command))
        {
            return ResolvePath(command, baseDirectory, effectivePathExtensions);
        }

        var resolutionKey = new ResolutionKey(
            command,
            baseDirectory,
            searchPath ?? Environment.GetEnvironmentVariable("PATH") ?? string.Empty,
            effectivePathExtensions);

        return ResolveNamedCommand(resolutionKey);
    }

    public static bool IsCommandScript(string path)
    {
        var extension = Path.GetExtension(path);
        return extension.Equals(".bat", StringComparison.OrdinalIgnoreCase)
               || extension.Equals(".cmd", StringComparison.OrdinalIgnoreCase);
    }

    private static string? ResolveNamedCommand(ResolutionKey resolutionKey)
    {
        if (ResolvedCommands.TryGetValue(resolutionKey, out var cachedPath) && File.Exists(cachedPath))
        {
            return cachedPath;
        }

        var resolvedPath = FindNamedCommand(resolutionKey);
        if (resolvedPath is not null)
        {
            ResolvedCommands[resolutionKey] = resolvedPath;
        }

        return resolvedPath;
    }

    private static string? FindNamedCommand(ResolutionKey resolutionKey)
    {
        var commandNames = GetCommandNames(resolutionKey.Command, resolutionKey.PathExtensions);
        foreach (var directory in GetSearchDirectories(resolutionKey.ProcessDirectory, resolutionKey.SearchPath))
        {
            foreach (var commandName in commandNames)
            {
                var candidate = Path.Combine(directory, commandName);
                if (File.Exists(candidate))
                {
                    return Path.GetFullPath(candidate);
                }
            }
        }

        return null;
    }

    private static IReadOnlyList<string> GetCommandNames(string command, string pathExtensions)
    {
        if (Path.HasExtension(command))
        {
            return [command];
        }

        return pathExtensions
            .Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(extension => extension.StartsWith('.') ? extension : $".{extension}")
            .Select(extension => command + extension.ToLowerInvariant())
            .ToList();
    }

    private static IEnumerable<string> GetSearchDirectories(string processDirectory, string searchPath)
    {
        yield return processDirectory;

        foreach (var directory in searchPath.Split(
                     Path.PathSeparator,
                     StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            yield return Path.GetFullPath(directory.Trim('"'), processDirectory);
        }
    }

    private static bool IsPath(string command) =>
        Path.IsPathRooted(command)
        || command.Contains(Path.DirectorySeparatorChar)
        || command.Contains(Path.AltDirectorySeparatorChar);

    private static string? ResolvePath(string command, string processDirectory, string pathExtensions)
    {
        var fullPath = Path.GetFullPath(command, processDirectory);
        if (File.Exists(fullPath))
        {
            return fullPath;
        }

        if (Path.HasExtension(fullPath))
        {
            return null;
        }

        return GetCommandNames(fullPath, pathExtensions)
            .FirstOrDefault(File.Exists);
    }

    private readonly record struct ResolutionKey(
        string Command,
        string ProcessDirectory,
        string SearchPath,
        string PathExtensions);
}
