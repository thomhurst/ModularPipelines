using System.Collections;

namespace ModularPipelines.Context;

internal class EnvironmentVariables : IEnvironmentVariablesContext
{
    private const string PathVariableName = "PATH";

    private static readonly Lock PathLock = new();

    private static StringComparer Comparer => OperatingSystem.IsWindows()
        ? StringComparer.OrdinalIgnoreCase
        : StringComparer.Ordinal;

    public string? Get(string name, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariable(name, target);
    }

    public IReadOnlyDictionary<string, string?> GetAll(
        EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Environment.GetEnvironmentVariables(target)
            .Cast<DictionaryEntry>()
            .ToDictionary(variable => variable.Key.ToString()!, variable => variable.Value?.ToString(), Comparer);
    }

    public void Set(string name, string? value, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        Environment.SetEnvironmentVariable(name, value, target);
    }

    public IReadOnlyList<string> GetPath(EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        return Get(PathVariableName, target)?.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries) ?? [];
    }

    public void AddToPath(string pathToAdd, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        AddToPath(pathToAdd, prepend: false, target);
    }

    public void AddToPath(string pathToAdd, bool prepend, EnvironmentVariableTarget target = EnvironmentVariableTarget.Process)
    {
        ArgumentNullException.ThrowIfNull(pathToAdd);

        lock (PathLock)
        {
            var existingPaths = GetPath(target);
            var newPaths = pathToAdd.Split(Path.PathSeparator, StringSplitOptions.RemoveEmptyEntries);
            var paths = prepend ? newPaths.Concat(existingPaths) : existingPaths.Concat(newPaths);

            Set(PathVariableName, string.Join(Path.PathSeparator, paths.Distinct(Comparer)), target);
        }
    }
}
