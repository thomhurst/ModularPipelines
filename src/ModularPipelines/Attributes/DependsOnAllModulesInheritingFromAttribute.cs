using ModularPipelines.Exceptions;
using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class DependsOnAllModulesInheritingFromAttribute : Attribute
{
    public DependsOnAllModulesInheritingFromAttribute(Type type)
    {
        if (!type.IsAssignableTo(typeof(IModule)))
        {
            throw new InvalidModuleTypeException(type);
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