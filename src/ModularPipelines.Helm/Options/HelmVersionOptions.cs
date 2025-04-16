using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("version")]
[ExcludeFromCodeCoverage]
public record HelmVersionOptions : HelmOptions
{
    [BooleanCommandSwitch("--short")]
    public virtual bool? Short { get; set; }

    [CommandEqualsSeparatorSwitch("--template", SwitchValueSeparator = " ")]
    public string? Template { get; set; }
}