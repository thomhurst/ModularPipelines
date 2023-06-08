using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Engine;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;
// ReSharper disable SuggestBaseTypeForParameterInConstructor

namespace ModularPipelines.Context;

internal class ModuleContext<TSelfModule> : IModuleContext<TSelfModule>
{
    private readonly ModuleLogger<TSelfModule> _moduleLogger;
    public ILogger Logger => _moduleLogger;

    public IServiceProvider ServiceProvider { get; }

    public Type ModuleType { get; } = typeof(TSelfModule);

    public IConfiguration Configuration { get; }

    public IOptions<PipelineOptions> PipelineOptions { get; }

    public IDependencyCollisionDetector DependencyCollisionDetector { get; }

    public IEnvironmentContext Environment { get; }

    public IModuleResultRepository ModuleResultRepository { get; }
    public ICommand Command { get; }

    public T Get<T>()
    {
        return (T) ServiceProvider.GetRequiredService(typeof(T));
    }

    public IFileSystemContext FileSystem { get; }

    public ModuleContext(IServiceProvider serviceProvider, 
        IDependencyCollisionDetector dependencyCollisionDetector, 
        IEnvironmentContext environment, 
        IFileSystemContext fileSystem,
        IConfiguration configuration, 
        IOptions<PipelineOptions> pipelineOptions,
        ModuleLogger<TSelfModule> moduleLogger, 
        IModuleResultRepository moduleResultRepository,
        ICommand<TSelfModule> command)
    {
        _moduleLogger = moduleLogger;
        ModuleResultRepository = moduleResultRepository;
        Command = command;
        Configuration = configuration;
        PipelineOptions = pipelineOptions;
        ServiceProvider = serviceProvider;
        DependencyCollisionDetector = dependencyCollisionDetector;
        Environment = environment;
        FileSystem = fileSystem;
    }

    public TModule GetModule<TModule>() where TModule : ModuleBase
    {
        return ServiceProvider.GetServices<ModuleBase>().OfType<TModule>().Single();
    }

    public ModuleBase GetModule(Type type)
    {
        return ServiceProvider.GetServices<ModuleBase>().Single(module => module.GetType() == type);
    }
}