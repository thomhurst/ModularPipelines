using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute : Attribute
{
    public DependsOnAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new Exception($"{type.FullName} is not a Module (does not implement IModule)");
        }

        Type = type;
    }

    public Type Type { get; }

    public bool IgnoreIfNotRegistered { get; set; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<TModule> : DependsOnAttribute
    where TModule : IModule
{
    public DependsOnAttribute() : base(typeof(TModule))
    {
    }
}