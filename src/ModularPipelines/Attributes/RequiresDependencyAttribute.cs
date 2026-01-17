using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

/// <summary>
/// Declares a required dependency on another module. The pipeline will fail validation
/// if the dependency is not registered. This is equivalent to [DependsOn&lt;T&gt;(Optional = false)].
/// </summary>
/// <typeparam name="TModule">The module type that must be registered.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class RequiresDependencyAttribute<TModule> : DependsOnAttribute<TModule>
    where TModule : IModule
{
    public RequiresDependencyAttribute()
    {
        Optional = false;
    }
}
