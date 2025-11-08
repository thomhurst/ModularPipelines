using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

/// <summary>
/// Declares that a module depends on another module.
/// The dependent module will execute after all its dependencies complete successfully.
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute : Attribute
{
    public DependsOnAttribute(Type type)
    {
        // v3.0: Support both old ModuleBase and new IModule
        if (!type.IsAssignableTo(typeof(IModule)) && !type.IsAssignableTo(typeof(ModuleBase)))
        {
            throw new Exception($"{type.FullName} is not a Module class. It must implement IModule or inherit from ModuleBase.");
        }

        Type = type;
    }

    public Type Type { get; }

    /// <summary>
    /// If true, the dependency is optional. If the module is not registered, it will be ignored.
    /// If false (default), a ModuleNotRegisteredException will be thrown if the module is not registered.
    /// </summary>
    public bool IgnoreIfNotRegistered { get; set; }
}

/// <summary>
/// Generic version of DependsOnAttribute for type-safe dependency declaration.
/// </summary>
/// <typeparam name="TModule">The type of module this module depends on.</typeparam>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<TModule> : DependsOnAttribute
    where TModule : IModule
{
    public DependsOnAttribute() : base(typeof(TModule))
    {
    }
}