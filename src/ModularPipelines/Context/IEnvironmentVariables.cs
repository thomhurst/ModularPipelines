namespace ModularPipelines.Context;

public interface IEnvironmentVariables
{
    string? GetEnvironmentVariable(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    IDictionary<string, string> GetEnvironmentVariables(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    void SetEnvironmentVariable(string variableName, string value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    IReadOnlyList<string> GetPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);

    void AddToPath(string pathToAdd, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process);
}