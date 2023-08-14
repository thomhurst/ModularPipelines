namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class BooleanCommandSwitchAttribute : Attribute
{
    public string SwitchName { get; }

    public BooleanCommandSwitchAttribute(string switchName)
    {
        SwitchName = switchName;
    }
}
