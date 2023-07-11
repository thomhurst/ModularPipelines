using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("create")]
public record HelmCreateOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--starter", SwitchValueSeparator = " ")]
    public string? Starter { get; set; }
}
