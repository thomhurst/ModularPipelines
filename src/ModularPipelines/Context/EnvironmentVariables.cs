using System.Collections;
using ModularPipelines.Context;

namespace ModularPipelines.Context;

internal class EnvironmentVariables : IEnvironmentVariablesContext
{
    private const string PathVariableName = "PATH";

    private static char Delimiter => OperatingSystem.IsWindows() ? ';' : ':';

    public string? Get(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public IReadOnlyDictionary<string, string?> GetAll(
        EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariables(target)
            .Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value?.ToString());
    }

    public void Set(string name, string? value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        Environment.SetEnvironmentVariable(name, value, target);
    }

    public IReadOnlyList<string> GetPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Get(PathVariableName, target)?.Split(Delimiter) ?? [];
    }

    public void AddToPath(string pathToAdd, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        var oldValue = Get(PathVariableName, target);

        var newValue = $"{oldValue}{Delimiter}{pathToAdd}";

        Set(PathVariableName, newValue, target);
    }
}
