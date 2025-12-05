using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CliCommand("repo", "update")]
[ExcludeFromCodeCoverage]
public record HelmRepoUpdateOptions : HelmOptions
{
    [CliFlag("--fail-on-repo-update-fail")]
    public virtual bool? FailOnRepoUpdateFail { get; set; }
}