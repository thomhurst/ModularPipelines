using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("repo", "list")]
[ExcludeFromCodeCoverage]
public record HelmRepoListOptions : HelmOptions
{
    [CliOption("--output")]
    public virtual string? Output { get; set; }
}