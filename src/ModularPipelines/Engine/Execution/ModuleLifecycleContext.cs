using ModularPipelines.Context;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Context information required for invoking module lifecycle events.
/// </summary>
internal class ModuleLifecycleContext
{
    public ModuleLifecycleContext(
        IModule module,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        IPipelineContext pipelineContext,
        IServiceProvider scopedServiceProvider,
        CancellationToken cancellationToken)
    {
        Module = module;
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        StartTime = startTime;
        PipelineContext = pipelineContext;
        ScopedServiceProvider = scopedServiceProvider;
        CancellationToken = cancellationToken;
    }

    /// <summary>
    /// Gets the module instance.
    /// </summary>
    public IModule Module { get; }

    /// <summary>
    /// Gets the module type.
    /// </summary>
    public Type ModuleType { get; }

    /// <summary>
    /// Gets the module's custom attributes.
    /// </summary>
    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    /// <summary>
    /// Gets the start time of the module execution.
    /// </summary>
    public DateTimeOffset StartTime { get; }

    /// <summary>
    /// Gets the pipeline context.
    /// </summary>
    public IPipelineContext PipelineContext { get; }

    /// <summary>
    /// Gets the scoped service provider for this module execution.
    /// </summary>
    public IServiceProvider ScopedServiceProvider { get; }

    /// <summary>
    /// Gets the cancellation token.
    /// </summary>
    public CancellationToken CancellationToken { get; }

    /// <summary>
    /// Gets or sets the ready time (when dependencies were satisfied).
    /// </summary>
    public DateTimeOffset? ReadyTime { get; set; }
}
