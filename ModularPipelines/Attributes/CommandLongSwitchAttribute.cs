namespace ModularPipelines.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class CommandLongSwitchAttribute : CommandSwitchAttribute
{ 
    public CommandLongSwitchAttribute(string switchName) : base(switchName)
    {
        HyphenCount = 2;
        SwitchValueSeparator = "=";
    }
}