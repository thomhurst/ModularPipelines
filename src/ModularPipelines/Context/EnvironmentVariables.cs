using System.Collections;
using System.Runtime.InteropServices;

namespace ModularPipelines.Context;

internal class EnvironmentVariables : IEnvironmentVariables
{
    private static char Delimiter => (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) ? ';' : ':';

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
        return GetEnvironmentVariable("PATH", target)?.Split(Delimiter) ?? Array.Empty<string>();
    }

    public void AddToPath(string pathToAdd, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        const string pathVariableName = "PATH";

        var oldValue = Environment.GetEnvironmentVariable(pathVariableName, target);

        var newValue = $@"{oldValue}{Delimiter}{pathToAdd}";

        Environment.SetEnvironmentVariable(pathVariableName, newValue, target);
    }
}
