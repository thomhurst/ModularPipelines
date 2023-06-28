using System.Collections;

namespace ModularPipelines.Context;

public class EnvironmentVariables : IEnvironmentVariables
{
    public string? GetEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name);
    }

    public IDictionary<string, string> GetEnvironmentVariables()
    {
        return Environment.GetEnvironmentVariables()
            .Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value!.ToString()!);
    }

    public void SetEnvironmentVariable(string variableName, string value)
    {
        Environment.SetEnvironmentVariable(variableName, value);
    }

    public void AddToPath(string pathToAdd)
    {
        const string pathVariableName = "PATH";
        
        var oldValue = Environment.GetEnvironmentVariable(pathVariableName);
        
        var newValue  = $@"{oldValue};{pathToAdd}";
        
        Environment.SetEnvironmentVariable(pathVariableName, newValue);
    }
}