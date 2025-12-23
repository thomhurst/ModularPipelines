using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Attributes.Events;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Modules;

namespace ModularPipelines.Context;

/// <summary>
/// Implementation of <see cref="IModuleReadyContext"/>.
/// </summary>
internal class ModuleReadyContext : IModuleReadyContext
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IModuleMetadataRegistry _metadataRegistry;

    public ModuleReadyContext(
        IModule moduleInstance,
        Type moduleType,
        IReadOnlyList<Attribute> moduleAttributes,
        DateTimeOffset readyTime,
        DateTimeOffset pipelineStartTime,
        IPipelineContext pipelineContext,
        IServiceProvider serviceProvider,
        IModuleMetadataRegistry metadataRegistry,
        CancellationToken cancellationToken)
    {
        ModuleInstance = moduleInstance;
        ModuleType = moduleType;
        ModuleAttributes = moduleAttributes;
        ReadyTime = readyTime;
        TimeWaitingForDependencies = readyTime - pipelineStartTime;
        PipelineContext = pipelineContext;
        _serviceProvider = serviceProvider;
        _metadataRegistry = metadataRegistry;
        CancellationToken = cancellationToken;
    }

    public Type ModuleType { get; }

    public string ModuleName => ModuleType.Name;

    public IModule ModuleInstance { get; }

    public IReadOnlyList<Attribute> ModuleAttributes { get; }

    public DateTimeOffset ReadyTime { get; }

    public TimeSpan TimeWaitingForDependencies { get; }

    public IPipelineContext PipelineContext { get; }

    public CancellationToken CancellationToken { get; }

    public T GetService<T>() where T : notnull
        => _serviceProvider.GetRequiredService<T>();

    public T? GetServiceOrDefault<T>() where T : class
        => _serviceProvider.GetService<T>();

    public T? GetMetadata<T>(string key)
        => _metadataRegistry.GetMetadata<T>(ModuleType, key);
}
