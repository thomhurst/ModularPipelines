using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("search", "repo")]
[ExcludeFromCodeCoverage]
public record HelmSearchRepoOptions : HelmOptions
{
    [BooleanCommandSwitch("--devel")]
    public virtual bool? Devel { get; set; }

    [CommandEqualsSeparatorSwitch("--max-col-width", SwitchValueSeparator = " ")]
    public string? MaxColWidth { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }

    [BooleanCommandSwitch("--regexp")]
    public virtual bool? Regexp { get; set; }

    [CommandEqualsSeparatorSwitch("--version", SwitchValueSeparator = " ")]
    public string? Version { get; set; }

    [BooleanCommandSwitch("--versions")]
    public virtual bool? Versions { get; set; }
}