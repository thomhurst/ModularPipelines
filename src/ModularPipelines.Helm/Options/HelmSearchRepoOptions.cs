using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("search", "repo")]
[ExcludeFromCodeCoverage]
public record HelmSearchRepoOptions : HelmOptions
{
    [CliFlag("--devel")]
    public virtual bool? Devel { get; set; }

    [CliOption("--max-col-width")]
    public virtual string? MaxColWidth { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }

    [CliFlag("--regexp")]
    public virtual bool? Regexp { get; set; }

    [CliOption("--version")]
    public virtual string? Version { get; set; }

    [CliFlag("--versions")]
    public virtual bool? Versions { get; set; }
}