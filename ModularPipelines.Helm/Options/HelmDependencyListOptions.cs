using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("dependency", "list")]
public record HelmDependencyListOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--max-col-width", SwitchValueSeparator = " ")]
    public string? MaxColWidth { get; set; }
}
