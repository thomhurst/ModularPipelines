namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class BooleanCommandSwitchAttribute : Attribute, ICommandSwitchAttribute
{
    public BooleanCommandSwitchAttribute(string switchName)
    {
        SwitchName = switchName;
    }

    public string SwitchName { get; }
}