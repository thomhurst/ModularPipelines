using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class DependsOnAllModulesInheritingFromAttribute : Attribute
{
    public DependsOnAllModulesInheritingFromAttribute(Type type)
    {
        // v3.0: Accept IModule only (ModuleBase was removed)
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new Exception($"{type.FullName} is not a Module class");
        }

        Type = type;
    }

    public Type Type { get; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class DependsOnAllModulesInheritingFromAttribute<TModule> : DependsOnAllModulesInheritingFromAttribute
    where TModule : IModule
{
    public DependsOnAllModulesInheritingFromAttribute() : base(typeof(TModule))
    {
    }
}
