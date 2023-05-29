using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute : Attribute
{
    public Type Type { get; }

    public DependsOnAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new Exception($"{type.FullName} is not a Module class");
        }
        
        Type = type;
    }
}

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
public class DependsOnAttribute<TModule> : DependsOnAttribute where TModule : IModule
{
    public DependsOnAttribute() : base(typeof(TModule))
    {
    }
}