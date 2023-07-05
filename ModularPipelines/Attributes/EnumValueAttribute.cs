namespace ModularPipelines.Attributes;

[AttributeUsage( AttributeTargets.Field )]
public class EnumValueAttribute : Attribute
{
    public string Value { get; }

    public EnumValueAttribute( string value )
    {
        Value = value;
    }
}
