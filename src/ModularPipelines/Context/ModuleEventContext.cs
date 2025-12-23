using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Enums;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Implementation of <see cref="IModuleEventContext"/>.
/// </summary>
internal class ModuleEventContext : IModuleEventContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;
    private readonly DateTimeOffset _startTime;

    public ModuleEventContext(
        IModule moduleInstance,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset startTime,
        Status status,
        IPipelineContext pipelineContext,
        IServiceProvider serviceProvider,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken)
    {
        ModuleInstance = moduleInstance;
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        _startTime = startTime;
        Status = status;
        PipelineContext = pipelineContext;
        _serviceProvider = serviceProvider;
        _metadataRegistry = metadataRegistry;
        CancellationToken = cancellationToken;
    }

    public Type ModuleType { get; }

    public string ModuleName => ModuleType.Name;

    public IModule ModuleInstance { get; }

    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    public DateTimeOffset StartTime => _startTime;

    public TimeSpan ElapsedTime => DateTimeOffset.UtcNow - _startTime;

    public Status Status { get; }

    public IPipelineContext PipelineContext { get; }

    public CancellationToken CancellationToken { get; }

    // Control flow state
    public bool RetryRequested { get; private set; }

    public TimeSpan? RetryDelay { get; private set; }

    public bool ShouldSkipDependents { get; private set; }

    public string? SkipDependentsReason { get; private set; }

    public bool ShouldFailPipeline { get; private set; }

    public string? FailPipelineReason { get; private set; }

    public T GetService<T>() where T : notnull
        => _serviceProvider.GetRequiredService<T>();

    public T? GetServiceOrDefault<T>() where T : class
        => _serviceProvider.GetService<T>();

    public T? GetMetadata<T>(string key)
        => _metadataRegistry.GetMetadata<T>(ModuleType, key);

    public void RequestRetry(TimeSpan? delay = null)
    {
        RetryRequested = true;
        RetryDelay = delay;
    }

    public void SkipDependentModules(string reason)
    {
        ShouldSkipDependents = true;
        SkipDependentsReason = reason;
    }

    public void FailPipeline(string reason)
    {
        ShouldFailPipeline = true;
        FailPipelineReason = reason;
    }
}
