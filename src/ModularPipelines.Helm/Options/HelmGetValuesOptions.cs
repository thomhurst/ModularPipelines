using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("get", "values")]
[ExcludeFromCodeCoverage]
public record HelmGetValuesOptions : HelmOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }
}
