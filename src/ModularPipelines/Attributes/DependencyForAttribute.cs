using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependencyForAttribute : Attribute
{
    public DependencyForAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(ModuleBase)))
        {
            throw new Exception($"{type.FullName} is not a Module class");
        }

        Type = type;
    }

    public Type Type { get; }

    public bool IgnoreIfNotRegistered { get; set; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependencyForAttribute<TModule> : DependencyForAttribute
    where TModule : ModuleBase
{
    public DependencyForAttribute() : base(typeof(TModule))
    {
    }
}