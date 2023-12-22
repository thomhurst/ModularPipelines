using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("create")]
[ExcludeFromCodeCoverage]
public record HelmCreateOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--starter", SwitchValueSeparator = " ")]
    public string? Starter { get; set; }
}