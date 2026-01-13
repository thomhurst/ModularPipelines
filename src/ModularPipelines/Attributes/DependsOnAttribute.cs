using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute : Attribute
{
    [Obsolete("Use the generic DependsOnAttribute<TModule> instead for compile-time type safety. This constructor will be removed in a future version.")]
    public DependsOnAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new Exception($"{type.FullName} is not a Module (does not implement IModule)");
        }

        Type = type;
    }

    /// <summary>
    /// Internal constructor for use by the generic DependsOnAttribute to bypass the obsolete warning.
    /// </summary>
    internal DependsOnAttribute(Type type, bool skipValidation)
    {
        Type = type;
    }

    public Type Type { get; }

    public bool IgnoreIfNotRegistered { get; set; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<TModule> : DependsOnAttribute
    where TModule : IModule
{
    public DependsOnAttribute() : base(typeof(TModule), skipValidation: true)
    {
    }
}