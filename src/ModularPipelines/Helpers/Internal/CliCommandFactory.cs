using ModularPipelines.Options;

namespace ModularPipelines.Helpers.Internal;

internal static class CliCommandFactory
{
    private const string InternalEnvironmentVariablePrefix = "MODULAR_PIPELINES_CMD_";

    public static PreparedCommand Create(
        string tool,
        IEnumerable<string> arguments,
        CommandExecutionOptions? executionOptions = null)
    {
        var argumentList = arguments.ToList();
        // Resolve once with the command's own working directory, PATH and PATHEXT, and use the result both to
        // choose the COMSPEC wrapper and as the file to start.
        var resolvedCommand = ResolveWindowsCommand(
            tool,
            executionOptions?.WorkingDirectory,
            executionOptions?.EnvironmentVariables);
        if (resolvedCommand is null || !WindowsCommandResolver.IsCommandScript(resolvedCommand))
        {
            var directCommand = new ProcessCommand(
                tool,
                argumentList,
                CommandLineArguments.Join(argumentList),
                ResolveWorkingDirectory(executionOptions),
                CopyEnvironmentVariables(executionOptions?.EnvironmentVariables),
                executionOptions?.CommandLineCredentials,
                resolvedCommand);

            return new PreparedCommand(directCommand, directCommand.ToString());
        }

        var environmentVariables = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);
        if (executionOptions?.EnvironmentVariables is not null)
        {
            foreach (var pair in executionOptions.EnvironmentVariables)
            {
                environmentVariables[pair.Key] = pair.Value;
            }
        }

        var variablePrefix = $"{InternalEnvironmentVariablePrefix}{Guid.NewGuid():N}_";
        var variableNames = new List<string>(argumentList.Count + 1);
        AddCommandValue(environmentVariables, variableNames, $"{variablePrefix}TOOL", resolvedCommand);

        for (var index = 0; index < argumentList.Count; index++)
        {
            AddCommandValue(environmentVariables, variableNames, $"{variablePrefix}ARG_{index}", argumentList[index]);
        }

        // cmd's /c parser leaves references to empty environment variables unexpanded.
        var invocation = string.Join(" ", variableNames.Select(name =>
            environmentVariables[name] == string.Empty ? "\"\"" : $"\"%{name}%\""));

        // cmd expands the entire line before running it. Clear the temporary values before the script
        // starts so that neither the script nor its descendants inherit argument values as environment variables.
        var clearVariables = string.Concat(variableNames.Select(name => $"set \"{name}=\"&"));

        var wrappedCommand = new ProcessCommand(
            Environment.GetEnvironmentVariable("COMSPEC") ?? "cmd.exe",
            ArgumentList: null,
            $"/d /s /v:off /c \"{clearVariables}{invocation}\"",
            ResolveWorkingDirectory(executionOptions),
            environmentVariables,
            executionOptions?.CommandLineCredentials);

        var displayCommand = $"{resolvedCommand} {CommandLineArguments.Join(argumentList)}";

        return new PreparedCommand(wrappedCommand, displayCommand);
    }

    internal static bool IsInternalEnvironmentVariable(string name) =>
        name.StartsWith(InternalEnvironmentVariablePrefix, StringComparison.OrdinalIgnoreCase);

    private static string ResolveWorkingDirectory(CommandExecutionOptions? executionOptions) =>
        executionOptions?.WorkingDirectory ?? Directory.GetCurrentDirectory();

    private static Dictionary<string, string?> CopyEnvironmentVariables(
        IReadOnlyDictionary<string, string?>? environmentVariables)
    {
        return environmentVariables is null
            ? []
            : environmentVariables.ToDictionary(pair => pair.Key, pair => pair.Value);
    }

    private static void AddCommandValue(
        Dictionary<string, string?> environmentVariables,
        List<string> variableNames,
        string variableName,
        string value)
    {
        if (value.Contains('\r') || value.Contains('\n'))
        {
            throw new ArgumentException(
                "Windows command script paths and arguments cannot contain CR or LF characters.",
                nameof(value));
        }

        variableNames.Add(variableName);
        environmentVariables[variableName] = value.Replace("\"", "\"\"");
    }

    internal static string? ResolveWindowsCommand(
        string tool,
        string? workingDirectory,
        IEnumerable<KeyValuePair<string, string?>>? environmentVariables)
    {
        var path = GetEnvironmentVariable(environmentVariables, "PATH")
            ?? Environment.GetEnvironmentVariable("PATH");
        var pathExtensions = GetEnvironmentVariable(environmentVariables, "PATHEXT")
            ?? Environment.GetEnvironmentVariable("PATHEXT");
        return WindowsCommandResolver.Resolve(
            tool,
            workingDirectory ?? Environment.CurrentDirectory,
            path,
            pathExtensions);
    }

    private static string? GetEnvironmentVariable(
        IEnumerable<KeyValuePair<string, string?>>? environmentVariables,
        string name)
    {
        return environmentVariables?
            .FirstOrDefault(pair => string.Equals(pair.Key, name, StringComparison.OrdinalIgnoreCase))
            .Value;
    }
}

internal readonly record struct PreparedCommand(ProcessCommand Command, string Input);
