using ModularPipelines.Exceptions;
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
            throw new InvalidModuleTypeException(type);
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

    /// <summary>
    /// When true (default), the dependency is optional - the module will run even if the dependency
    /// is not registered or is skipped. When false, the dependency is required and must be registered.
    /// </summary>
    public bool Optional { get; set; } = true;

    /// <summary>
    /// Obsolete. Use Optional instead. This property now maps to !Optional for backwards compatibility.
    /// </summary>
    [Obsolete("Use Optional instead. IgnoreIfNotRegistered = true is equivalent to Optional = true (the new default).")]
    public bool IgnoreIfNotRegistered
    {
        get => Optional;
        set => Optional = value;
    }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<TModule> : DependsOnAttribute
    where TModule : IModule
{
    public DependsOnAttribute() : base(typeof(TModule), skipValidation: true)
    {
    }
}