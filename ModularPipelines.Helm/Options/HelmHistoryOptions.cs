using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("history")]
public record HelmHistoryOptions : HelmOptions
{
    [CommandLongSwitch("max", SwitchValueSeparator = " ")]
    public int? Max { get; set; }

    [CommandLongSwitch("output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

}
