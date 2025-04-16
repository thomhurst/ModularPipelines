using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "update")]
[ExcludeFromCodeCoverage]
public record HelmRepoUpdateOptions : HelmOptions
{
    [BooleanCommandSwitch("--fail-on-repo-update-fail")]
    public virtual bool? FailOnRepoUpdateFail { get; set; }
}