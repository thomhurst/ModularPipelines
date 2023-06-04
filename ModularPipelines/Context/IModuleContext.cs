using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IModuleContext<out TSelfModule> : IModuleContext
{
}

public interface IModuleContext
{
    internal TModule GetModule<TModule>() where TModule : ModuleBase;
    internal ModuleBase GetModule(Type type);
    public IServiceProvider ServiceProvider { get; }
    public IConfiguration Configuration { get; }
    public IOptions<PipelineOptions> PipelineOptions { get; }
    internal IDependencyCollisionDetector DependencyCollisionDetector { get; }
    public IEnvironmentContext Environment { get; }
    public IFileSystemContext FileSystem { get; }
    public T? Get<T>();
    public ILogger Logger { get; }
    public Type ModuleType { get; }
}