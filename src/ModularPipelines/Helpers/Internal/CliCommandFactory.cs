using CliWrap;
using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

internal static class CliCommandFactory
{
    public static Command Create(
        string tool,
        IEnumerable<string> arguments,
        CommandExecutionOptions? executionOptions = null)
    {
        var argumentList = arguments.ToList();
        var commandScript = ResolveWindowsCommandScript(tool, executionOptions?.EnvironmentVariables);
        if (commandScript is null)
        {
            return ApplyEnvironmentVariables(
                Cli.Wrap(tool).WithArguments(argumentList),
                executionOptions?.EnvironmentVariables);
        }

        var environmentVariables = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        if (executionOptions?.EnvironmentVariables is not null)
        {
            foreach (var pair in executionOptions.EnvironmentVariables)
            {
                environmentVariables[pair.Key] = pair.Value;
            }
        }

        var variablePrefix = $"MODULAR_PIPELINES_CMD_{Guid.NewGuid():N}_";
        var variableNames = new List<string>(argumentList.Count + 1);
        AddCommandValue(environmentVariables, variableNames, $"{variablePrefix}TOOL", commandScript);

        for (var index = 0; index < argumentList.Count; index++)
        {
            AddCommandValue(environmentVariables, variableNames, $"{variablePrefix}ARG_{index}", argumentList[index]);
        }

        var invocation = string.Join(" ", variableNames.Select(name => $"\"%{name}%\""));

        return Cli.Wrap(Environment.GetEnvironmentVariable("COMSPEC") ?? "cmd.exe")
            .WithArguments($"/d /s /v:off /c \"{invocation}\"")
            .WithEnvironmentVariables(environmentVariables);
    }

    private static Command ApplyEnvironmentVariables(
        Command command,
        IDictionary<string, string?>? environmentVariables)
    {
        return environmentVariables is null
            ? command
            : command.WithEnvironmentVariables(new Dictionary<string, string?>(environmentVariables));
    }

    private static void AddCommandValue(
        IDictionary<string, string?> environmentVariables,
        ICollection<string> variableNames,
        string variableName,
        string value)
    {
        variableNames.Add(variableName);
        environmentVariables[variableName] = value.Replace("\"", "\"\"");
    }

    private static string? ResolveWindowsCommandScript(
        string tool,
        IEnumerable<KeyValuePair<string, string?>>? environmentVariables)
    {
        if (!OperatingSystem.IsWindows())
        {
            return null;
        }

        var extension = Path.GetExtension(tool);
        if (IsCommandScriptExtension(extension))
        {
            return tool;
        }

        if (!string.IsNullOrEmpty(extension) || tool.Contains(Path.DirectorySeparatorChar) || tool.Contains(Path.AltDirectorySeparatorChar))
        {
            return null;
        }

        var path = GetEnvironmentVariable(environmentVariables, "PATH")
            ?? Environment.GetEnvironmentVariable("PATH");
        var pathExtensions = GetEnvironmentVariable(environmentVariables, "PATHEXT")
            ?? Environment.GetEnvironmentVariable("PATHEXT")
            ?? ".COM;.EXE;.BAT;.CMD";

        foreach (var directory in (path ?? string.Empty).Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
        {
            foreach (var candidateExtension in pathExtensions.Split(';', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
            {
                var candidate = Path.Combine(directory.Trim('"'), tool + candidateExtension);
                if (!File.Exists(candidate))
                {
                    continue;
                }

                return IsCommandScriptExtension(candidateExtension) ? candidate : null;
            }
        }

        return null;
    }

    private static string? GetEnvironmentVariable(
        IEnumerable<KeyValuePair<string, string?>>? environmentVariables,
        string name)
    {
        return environmentVariables?
            .FirstOrDefault(pair => string.Equals(pair.Key, name, StringComparison.OrdinalIgnoreCase))
            .Value;
    }

    private static bool IsCommandScriptExtension(string extension)
    {
        return extension.Equals(".bat", StringComparison.OrdinalIgnoreCase) ||
               extension.Equals(".cmd", StringComparison.OrdinalIgnoreCase);
    }
}
