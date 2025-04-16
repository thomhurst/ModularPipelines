using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("status")]
[ExcludeFromCodeCoverage]
public record HelmStatusOptions : HelmOptions
{
    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [CommandEqualsSeparatorSwitch("--revision", SwitchValueSeparator = " ")]
    public int? Revision { get; set; }

    [BooleanCommandSwitch("--show-desc")]
    public virtual bool? ShowDesc { get; set; }

    [BooleanCommandSwitch("--show-resources")]
    public virtual bool? ShowResources { get; set; }
}