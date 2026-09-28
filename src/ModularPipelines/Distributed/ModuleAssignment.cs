namespace ModularPipelines.Distributed;

/// <summary>
/// Describes a module the master has queued for execution by a distributed worker.
/// </summary>
/// <remarks>
/// Wire types use <see langword="required"/> and <see langword="init"/> properties so later
/// versions can add optional members without breaking serialized payloads or callers.
/// </remarks>
public sealed record ModuleAssignment
{
    /// <summary>Gets the module to execute.</summary>
    public required ModuleId ModuleId { get; init; }

    /// <summary>Gets the capabilities a worker must advertise to claim the assignment.</summary>
    public required CapabilityRequirement RequiredCapabilities { get; init; }

    /// <summary>Gets the schema expected by the process that issued this assignment.</summary>
    public required string PipelineSchemaVersion { get; init; }

    /// <summary>
    /// Gets whether the module keeps running after the pipeline fails. Workers continue to claim
    /// and execute AlwaysRun assignments after a <see cref="DistributedCancellationReason.PipelineFailed"/>
    /// broadcast; other assignments are withdrawn.
    /// </summary>
    public bool AlwaysRun { get; init; }

    /// <summary>Gets the user-configured scheduling priority.</summary>
    public ModulePriority Priority { get; init; } = ModulePriority.Normal;

    /// <summary>Gets the estimated duration of the longest downstream path starting at this module.</summary>
    public TimeSpan CriticalPathWeight { get; init; }

    /// <summary>Gets when the master enqueued the assignment, according to the master's clock.</summary>
    public DateTimeOffset EnqueuedAt { get; init; }

    /// <summary>
    /// Gets the dependencies whose stored results the executing worker applies before it runs the module.
    /// </summary>
    public IReadOnlyList<DependencyResultReference> DependencyResultReferences { get; init; } = [];

    /// <summary>
    /// Gets the condition groups the master already evaluated as satisfied for this module, so the
    /// worker does not evaluate them again. Each entry is a version-independent type name in the form
    /// <c>Namespace.TypeName, AssemblyName</c>.
    /// </summary>
    public IReadOnlyList<string> SatisfiedConditionGroups { get; init; } = [];

    /// <summary>
    /// Gets the names of <see cref="Attributes.ProducesArtifactAttribute"/> artifacts that planned
    /// consumers require. The worker fails the module when one of them is not produced.
    /// </summary>
    public IReadOnlyList<string> RequiredArtifacts { get; init; } = [];

    /// <summary>
    /// Gets the artifacts, taken from accepted producer results, that this module consumes through
    /// <see cref="Attributes.ConsumesArtifactAttribute"/>. Workers download exactly these references.
    /// </summary>
    public IReadOnlyList<ArtifactReference> ConsumedArtifacts { get; init; } = [];
}
