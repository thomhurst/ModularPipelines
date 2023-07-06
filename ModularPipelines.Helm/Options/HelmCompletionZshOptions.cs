using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "zsh")]
public record HelmCompletionZshOptions : HelmOptions
{
    [BooleanCommandSwitch("no-descriptions")]
    public bool? NoDescriptions { get; set; }

}
