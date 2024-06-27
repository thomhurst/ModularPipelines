using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class DependsOnAllModulesInheritingFromAttribute : Attribute
{
    public DependsOnAllModulesInheritingFromAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(ModuleBase)))
        {
            throw new Exception($"{type.FullName} is not a Module class");
        }

        Type = type;
    }

    public Type Type { get; }
}

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class DependsOnAllModulesInheritingFromAttribute<TModule> : DependsOnAllModulesInheritingFromAttribute
    where TModule : ModuleBase
{
    public DependsOnAllModulesInheritingFromAttribute() : base(typeof(TModule))
    {
    }
}