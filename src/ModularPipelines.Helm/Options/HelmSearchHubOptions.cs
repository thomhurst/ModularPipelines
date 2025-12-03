using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("search", "hub")]
[ExcludeFromCodeCoverage]
public record HelmSearchHubOptions : HelmOptions
{
    [CliOption("--endpoint")]
    public string? Endpoint { get; set; }

    [CliFlag("--list-repo-url")]
    public virtual bool? ListRepoUrl { get; set; }

    [CliOption("--max-col-width")]
    public string? MaxColWidth { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }
}