using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("get", "notes")]
[ExcludeFromCodeCoverage]
public record HelmGetNotesOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }
}
