namespace ModularPipelines.Context;

public interface IEnvironmentVariables
{
    string? GetEnvironmentVariable(string name);

    IDictionary<string, string> GetEnvironmentVariables();

    void SetEnvironmentVariable(string variableName, string value);

    void AddToPath(string pathToAdd);
}
