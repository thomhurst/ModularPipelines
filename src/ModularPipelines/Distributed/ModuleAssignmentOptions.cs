namespace ModularPipelines.Distributed;

/// <summary>
/// Contains the portable module settings used by distributed coordination.
/// </summary>
/// <remarks>
/// Retry policies are resolved on the executing node from the module's configuration and
/// <see cref="Options.PipelineOptions.DefaultRetryCount"/>. This preserves declarative retries and
/// node-local resilience shield factories without attempting to serialize delegates.
/// </remarks>
public record ModuleAssignmentOptions(
    double? TimeoutSeconds,
    bool AlwaysRun);
