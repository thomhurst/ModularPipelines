using Microsoft.Extensions.DependencyInjection;

namespace Pipeline.NET;

internal class ModuleContext : IModuleContext
{
    public IServiceProvider ServiceProvider { get; }
    public IDependencyCollisionDetector DependencyCollisionDetector { get; }
    public IEnvironmentContext Environment { get; }

    public ModuleContext(IServiceProvider serviceProvider, IDependencyCollisionDetector dependencyCollisionDetector, IEnvironmentContext environment)
    {
        ServiceProvider = serviceProvider;
        DependencyCollisionDetector = dependencyCollisionDetector;
        Environment = environment;
    }

    public TModule GetModule<TModule>() where TModule : IModule
    {
        return ServiceProvider.GetServices<IModule>().OfType<TModule>().Single();
    }
}