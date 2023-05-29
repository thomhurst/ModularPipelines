using System.Collections.Concurrent;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

internal class ModuleContext : IModuleContext
{
    private readonly ILogger<ModuleContext> _logger;
    private readonly ConcurrentDictionary<Type, object> _resolvedInstances = new();

    public IServiceProvider ServiceProvider { get; }

    public ILogger Logger => _logger;

    public IConfiguration Configuration { get; }

    public IOptions<PipelineOptions> PipelineOptions { get; }

    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IEnvironmentContext Environment { get; }

    public T? Get<T>()
    {
        return ServiceProvider.GetService<T>()
               ?? (T) _resolvedInstances.GetOrAdd(typeof(T),
                   _ => ActivatorUtilities.GetServiceOrCreateInstance<T>(ServiceProvider)!);
    }

    public IFileSystemContext FileSystem { get; }

    public ModuleContext(IServiceProvider serviceProvider, 
        IDependencyCollisionDetector dependencyCollisionDetector, 
        IEnvironmentContext environment, 
        IFileSystemContext fileSystem,
        ILogger<ModuleContext> logger, 
        IConfiguration configuration, 
        IOptions<PipelineOptions> pipelineOptions)
    {
        _logger = logger;
        Configuration = configuration;
        PipelineOptions = pipelineOptions;
        ServiceProvider = serviceProvider;
        DependencyCollisionDetector = dependencyCollisionDetector;
        Environment = environment;
        FileSystem = fileSystem;
    }

    public TModule GetModule<TModule>() where TModule : IModule
    {
        return ServiceProvider.GetServices<IModule>().OfType<TModule>().Single();
    }

    public IModule GetModule(Type type)
    {
        return ServiceProvider.GetServices<IModule>().Single(module => module.GetType() == type);
    }
}