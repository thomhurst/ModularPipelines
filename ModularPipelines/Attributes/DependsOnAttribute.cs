using ModularPipelines.Modules;

namespace ModularPipelines.Attributes;

[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true )]
public class DependsOnAttribute : Attribute
{
    public Type Type { get; }

    public DependsOnAttribute( Type type )
    {
        if (!type.IsAssignableTo( typeof( ModuleBase ) ))
        {
            throw new Exception( $"{type.FullName} is not a Module class" );
        }

        Type = type;
    }

    public bool IgnoreIfNotRegistered { get; set; }
}

[AttributeUsage( AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true, Inherited = true )]
public class DependsOnAttribute<TModule> : DependsOnAttribute where TModule : ModuleBase
{
    public DependsOnAttribute() : base( typeof( TModule ) )
    {
    }
}
