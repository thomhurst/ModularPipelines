using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("get", "manifest")]
[ExcludeFromCodeCoverage]
public record HelmGetManifestOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }
}
