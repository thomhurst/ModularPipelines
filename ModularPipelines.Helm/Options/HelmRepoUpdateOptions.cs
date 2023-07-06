using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("repo", "update")]
public record HelmRepoUpdateOptions : HelmOptions
{
    [BooleanCommandSwitch("fail-on-repo-update-fail")]
    public bool? FailOnRepoUpdateFail { get; set; }

}
