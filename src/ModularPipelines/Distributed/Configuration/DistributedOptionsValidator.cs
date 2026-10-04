namespace ModularPipelines.Distributed.Configuration;

/// <summary>
/// Validates artifact options in all modes and execution options when distributed mode is enabled.
/// Pipeline validation reports the failures, and the distributed execution backend refuses to start with them.
/// </summary>
internal static class DistributedOptionsValidator
{
    public static IReadOnlyList<string> Validate(DistributedOptions options)
    {
        var failures = new List<string>();
        if (!Enum.IsDefined(options.ArtifactCompressionLevel))
        {
            failures.Add("Distributed.ArtifactCompressionLevel must be a defined CompressionLevel value.");
        }

        if (!options.Enabled)
        {
            return failures;
        }

        ValidateExecutionOptions(options, failures);
        return failures;
    }

    private static void ValidateExecutionOptions(DistributedOptions options, List<string> failures)
    {
        if (options.TotalInstances < 1)
        {
            failures.Add("Distributed.TotalInstances must be at least 1.");
        }

        if (options.InstanceIndex < 0 || options.InstanceIndex >= options.TotalInstances)
        {
            failures.Add(
                $"Distributed.InstanceIndex ({options.InstanceIndex}) must be at least zero and less than "
                + $"Distributed.TotalInstances ({options.TotalInstances}).");
        }

        if (options.MaxParallelism is < 1)
        {
            failures.Add("Distributed.MaxParallelism must be at least 1 when set.");
        }

        var expectedWorkers = Math.Max(0, options.TotalInstances - 1);
        if (options.MinimumWorkerCount < 0 || options.MinimumWorkerCount > expectedWorkers)
        {
            failures.Add(
                $"Distributed.MinimumWorkerCount must be between zero and {expectedWorkers} "
                + "for the configured total instance count.");
        }

        if (options.WorkerHeartbeatInterval <= TimeSpan.Zero)
        {
            failures.Add("Distributed.WorkerHeartbeatInterval must be positive.");
        }

        if (options.WorkerTimeout <= options.WorkerHeartbeatInterval)
        {
            failures.Add(
                $"Distributed.WorkerTimeout must exceed {nameof(DistributedOptions.WorkerHeartbeatInterval)}.");
        }

        if (options.MasterTimeout <= options.WorkerHeartbeatInterval)
        {
            failures.Add(
                $"Distributed.MasterTimeout must exceed {nameof(DistributedOptions.WorkerHeartbeatInterval)}.");
        }

        if (options.WorkerRegistrationTimeout < TimeSpan.Zero)
        {
            failures.Add("Distributed.WorkerRegistrationTimeout cannot be negative.");
        }

        if (options.ModuleResultTimeout < TimeSpan.Zero)
        {
            failures.Add("Distributed.ModuleResultTimeout cannot be negative.");
        }
    }
}
