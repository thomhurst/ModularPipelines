namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class BooleanCommandSwitchAttribute : Attribute
{
    public string SwitchName { get; }
    public int HyphenCount { get; set; } = 2;

    public BooleanCommandSwitchAttribute(string switchName)
    {
        SwitchName = switchName;
    }
}