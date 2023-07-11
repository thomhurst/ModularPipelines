using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("version")]
public record HelmVersionOptions : HelmOptions
{
    [BooleanCommandSwitch("--short")]
    public bool? Short { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }

}
