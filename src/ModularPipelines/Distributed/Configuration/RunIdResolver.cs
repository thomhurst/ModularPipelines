namespace ModularPipelines.Distributed.Configuration;

internal static class RunIdResolver
{
    internal const string EnvironmentVariable = "MODULARPIPELINES_RUN_ID";

    public static string Resolve(
        string? configuredValue,
        int totalInstances,
        bool requireExplicitRunId = false)
    {
        if (!string.IsNullOrWhiteSpace(configuredValue))
        {
            return Validate(configuredValue, nameof(DistributedOptions.RunId));
        }

        var environmentValue = Environment.GetEnvironmentVariable(EnvironmentVariable);
        if (!string.IsNullOrWhiteSpace(environmentValue))
        {
            return Validate(environmentValue, EnvironmentVariable);
        }

        if (totalInstances > 1 || requireExplicitRunId)
        {
            throw new InvalidOperationException(
                $"This distributed configuration requires one shared {nameof(DistributedOptions.RunId)}. "
                + "Configure it explicitly "
                + $"or set {EnvironmentVariable} for every process.");
        }

        return Guid.NewGuid().ToString("N");
    }

    // Backends embed the run identifier in Redis hash tags, object keys and URLs, where
    // characters such as '}', '/' or ':' would change slotting or key structure.
    private static string Validate(string runId, string source)
    {
        if (!DistributedIdentifier.IsValid(runId))
        {
            throw new InvalidOperationException(
                $"The run identifier from {source} ('{runId}') is invalid. Run identifiers must contain 1-"
                + $"{DistributedIdentifier.MaximumLength} {DistributedIdentifier.AllowedCharactersDescription}.");
        }

        return runId;
    }
}
