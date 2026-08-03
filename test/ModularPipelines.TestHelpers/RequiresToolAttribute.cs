using System.Collections.Concurrent;

namespace ModularPipelines.TestHelpers;

public sealed class RequiresToolAttribute : SkipAttribute
{
    private static readonly ConcurrentDictionary<string, Lazy<bool>> ToolAvailability =
        new(OperatingSystem.IsWindows() ? StringComparer.OrdinalIgnoreCase : StringComparer.Ordinal);

    private readonly string _tool;

    public RequiresToolAttribute(string tool)
        : base($"Requires tool '{tool}' on PATH")
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(tool);
        _tool = tool;
    }

    public override Task<bool> ShouldSkip(TestRegisteredContext context)
    {
        var isAvailable = ToolAvailability.GetOrAdd(
            _tool,
            static tool => new Lazy<bool>(() => IsAvailableOnPath(tool)));

        return Task.FromResult(!isAvailable.Value);
    }

    private static bool IsAvailableOnPath(string tool)
    {
        var path = Environment.GetEnvironmentVariable("PATH");
        if (string.IsNullOrWhiteSpace(path))
        {
            return false;
        }

        var candidateNames = GetCandidateNames(tool);
        return path.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Select(entry => entry.Trim('"'))
            .Where(entry => entry.Length > 0)
            .Any(entry => candidateNames.Any(candidateName => IsExecutable(Path.Combine(entry, candidateName))));
    }

    private static string[] GetCandidateNames(string tool)
    {
        if (!OperatingSystem.IsWindows())
        {
            return [tool];
        }

        var pathExtensions = Environment.GetEnvironmentVariable("PATHEXT") ?? ".COM;.EXE;.BAT;.CMD";
        return
        [
            tool,
            .. pathExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(extension => tool + extension),
        ];
    }

    private static bool IsExecutable(string path)
    {
        if (!File.Exists(path))
        {
            return false;
        }

        if (OperatingSystem.IsWindows())
        {
            return true;
        }

        try
        {
            const UnixFileMode executeBits = UnixFileMode.UserExecute |
                                             UnixFileMode.GroupExecute |
                                             UnixFileMode.OtherExecute;
            return (File.GetUnixFileMode(path) & executeBits) != 0;
        }
        catch (IOException)
        {
            return false;
        }
        catch (UnauthorizedAccessException)
        {
            return false;
        }
    }
}
