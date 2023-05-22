namespace Pipeline.NET;

public interface IModuleContext
{
    internal TModule GetModule<TModule>() where TModule : IModule;
    public IServiceProvider ServiceProvider { get; }
    internal IDependencyCollisionDetector DependencyCollisionDetector { get; }
    public IEnvironmentContext Environment { get; }
}