namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CommandEqualsSeparatorSwitchAttribute : CommandSwitchAttribute
{
    public CommandEqualsSeparatorSwitchAttribute(string switchName) : base(switchName)
    {
        SwitchValueSeparator = "=";
    }
}
