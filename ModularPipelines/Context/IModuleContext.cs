using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Helpers;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Context;

public interface IModuleContext
{
    internal TModule GetModule<TModule>() where TModule : IModule;
    internal IModule GetModule(Type type);
    public IServiceProvider ServiceProvider { get; }
    public ILogger Logger { get; }
    public IConfiguration Configuration { get; }
    public IOptions<PipelineOptions> PipelineOptions { get; }
    internal IDependencyCollisionDetector DependencyCollisionDetector { get; }
    public IEnvironmentContext Environment { get; }
    public IFileSystemContext FileSystem { get; }
    public T? Get<T>();
}