using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("repo", "index")]
[ExcludeFromCodeCoverage]
public record HelmRepoIndexOptions : HelmOptions
{
    [CliOption("--merge")]
    public string? Merge { get; set; }

    [CliOption("--url")]
    public string? Url { get; set; }
}