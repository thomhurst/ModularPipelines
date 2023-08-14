namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CommandSwitchAttribute : Attribute
{
    public string SwitchName { get; }
    public string SwitchValueSeparator { get; init; } = " ";

    public CommandSwitchAttribute(string switchName)
    {
        SwitchName = switchName;
    }
}