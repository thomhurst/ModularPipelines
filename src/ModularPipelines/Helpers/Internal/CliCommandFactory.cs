using CliWrap;
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
        var commandScript = ResolveWindowsCommandScript(
            tool,
            executionOptions?.WorkingDirectory,
            executionOptions?.EnvironmentVariables);
        if (commandScript is null)
        {
            var directCommand = ApplyEnvironmentVariables(
                Cli.Wrap(tool).WithArguments(argumentList),
                executionOptions?.EnvironmentVariables);

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
        AddCommandValue(environmentVariables, variableNames, $"{variablePrefix}TOOL", commandScript);

        for (var index = 0; index < argumentList.Count; index++)
        {
            AddCommandValue(environmentVariables, variableNames, $"{variablePrefix}ARG_{index}", argumentList[index]);
        }

        var invocation = string.Join(" ", variableNames.Select(name => $"\"%{name}%\""));

        var wrappedCommand = Cli.Wrap(Environment.GetEnvironmentVariable("COMSPEC") ?? "cmd.exe")
            .WithArguments($"/d /s /v:off /c \"{invocation}\"")
            .WithEnvironmentVariables(environmentVariables);

        var displayCommand = Cli.Wrap(commandScript)
            .WithArguments(argumentList)
            .ToString();

        return new PreparedCommand(wrappedCommand, displayCommand);
    }

    internal static bool IsInternalEnvironmentVariable(string name) =>
        name.StartsWith(InternalEnvironmentVariablePrefix, StringComparison.OrdinalIgnoreCase);

    private static Command ApplyEnvironmentVariables(
        Command command,
        IReadOnlyDictionary<string, string?>? environmentVariables)
    {
        return environmentVariables is null
            ? command
            : command.WithEnvironmentVariables(
                environmentVariables.ToDictionary(pair => pair.Key, pair => pair.Value));
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

    private static string? ResolveWindowsCommandScript(
        string tool,
        string? workingDirectory,
        IEnumerable<KeyValuePair<string, string?>>? environmentVariables)
    {
        var path = GetEnvironmentVariable(environmentVariables, "PATH")
            ?? Environment.GetEnvironmentVariable("PATH");
        var pathExtensions = GetEnvironmentVariable(environmentVariables, "PATHEXT")
            ?? Environment.GetEnvironmentVariable("PATHEXT");
        var resolvedCommand = WindowsCommandResolver.Resolve(
            tool,
            workingDirectory ?? Environment.CurrentDirectory,
            path,
            pathExtensions);

        return resolvedCommand is not null && WindowsCommandResolver.IsCommandScript(resolvedCommand)
            ? resolvedCommand
            : null;
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

internal readonly record struct PreparedCommand(Command Command, string Input);
