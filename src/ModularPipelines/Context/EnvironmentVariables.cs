using System.Collections;

namespace ModularPipelines.Context;

internal class EnvironmentVariables : IEnvironmentVariables
{
    private const string PathVariableName = "PATH";

    private static char Delimiter => OperatingSystem.IsWindows() ? ';' : ':';

    public string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariables(target)
            .Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value!.ToString()!);
    }

    public void SetEnvironmentVariable(string variableName, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        Environment.SetEnvironmentVariable(variableName, value, target);
    }

    public IReadOnlyList<string> GetPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return GetEnvironmentVariable(PathVariableName, target)?.Split(Delimiter) ?? [];
    }

    public void AddToPath(string pathToAdd, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        var oldValue = Environment.GetEnvironmentVariable(PathVariableName, target);

        var newValue = $"{oldValue}{Delimiter}{pathToAdd}";

        Environment.SetEnvironmentVariable(PathVariableName, newValue, target);
    }
}