namespace ModularPipelines.Distributed.Configuration;

internal static class RunIdResolver
{
    internal const string EnvironmentVariable = "MODULARPIPELINES_RUN_ID";

    public static string Resolve(string? configuredValue, int totalInstances)
    {
        if (!string.IsNullOrWhiteSpace(configuredValue))
        {
            return configuredValue;
        }

        var environmentValue = Environment.GetEnvironmentVariable(EnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(environmentValue))
        {
            return environmentValue;
        }

        if (totalInstances > 1)
        {
            throw new InvalidOperationException(
                $"Distributed execution with {nameof(DistributedOptions.TotalInstances)} greater than 1 "
                + $"requires one shared {nameof(DistributedOptions.RunId)}. Configure it explicitly "
                + $"or set {EnvironmentVariable} for every process.");
        }

        return Guid.NewGuid().ToString("N");
    }
}
