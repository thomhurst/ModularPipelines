namespace ModularPipelines.OptionsGenerator.TypeDetection;

internal static class ExecutableOverrideValidator
{
    public static void Validate(string tools, string? executableOverride)
    {
        if (string.IsNullOrWhiteSpace(executableOverride))
        {
            return;
        }

        var requestedTools = tools.Split(
            ',',
            StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        if (requestedTools.Length == 1
            && !string.Equals(requestedTools[0], "all", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        throw new InvalidOperationException(
            $"{ProcessCliCommandExecutor.ExecutableOverrideVariableName} requires exactly one explicit tool.");
    }
}
