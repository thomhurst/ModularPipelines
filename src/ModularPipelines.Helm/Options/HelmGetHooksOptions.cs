using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("get", "hooks")]
[ExcludeFromCodeCoverage]
public record HelmGetHooksOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }
}
