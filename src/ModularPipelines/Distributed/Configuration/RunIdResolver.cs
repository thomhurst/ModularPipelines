namespace ModularPipelines.Distributed.Configuration;

internal static class RunIdResolver
{
    internal const string EnvironmentVariable = "RUN_IDENTIFIER";

    public static string Resolve(string? configuredValue)
    {
        if (!string.IsNullOrWhiteSpace(configuredValue))
        {
            return configuredValue;
        }

        var environmentValue = Environment.GetEnvironmentVariable(EnvironmentVariable);
        return string.IsNullOrWhiteSpace(environmentValue)
            ? Guid.NewGuid().ToString("N")
            : environmentValue;
    }
}
