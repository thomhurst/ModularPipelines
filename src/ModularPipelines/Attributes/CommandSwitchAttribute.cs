namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CommandSwitchAttribute : Attribute
{
    public CommandSwitchAttribute(string switchName)
    {
        SwitchName = switchName;
    }

    public string SwitchName { get; }

    public string SwitchValueSeparator { get; init; } = " ";
}