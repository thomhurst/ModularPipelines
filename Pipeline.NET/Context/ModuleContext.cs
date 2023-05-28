using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Pipeline.NET.Helpers;
using Pipeline.NET.Modules;
using Pipeline.NET.Options;

namespace Pipeline.NET.Context;

internal class ModuleContext : IModuleContext
{
    private readonly ILogger<ModuleContext> _logger;

    public IServiceProvider ServiceProvider { get; }

    public ILogger Logger => _logger;

    public IConfiguration Configuration { get; }

    public IOptions<PipelineOptions> PipelineOptions { get; }

    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IEnvironmentContext Environment { get; }

    public T? Get<T>() => ServiceProvider.GetService<T>();

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
        return ServiceProvider.GetServices(type).OfType<IModule>().Single();
    }
}