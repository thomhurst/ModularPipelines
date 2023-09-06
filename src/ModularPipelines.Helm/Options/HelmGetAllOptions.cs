using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("get", "all")]
[ExcludeFromCodeCoverage]
public record HelmGetAllOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }
}
