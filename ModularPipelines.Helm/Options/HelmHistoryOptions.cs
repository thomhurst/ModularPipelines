using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("history")]
public record HelmHistoryOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--max", SwitchValueSeparator = " ")]
    public int? Max { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }
}
