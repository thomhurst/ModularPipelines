using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("repo", "index")]
[ExcludeFromCodeCoverage]
public record HelmRepoIndexOptions : HelmOptions
{
    [CliOption("--merge")]
    public virtual string? Merge { get; set; }

    [CliOption("--url")]
    public virtual string? Url { get; set; }
}