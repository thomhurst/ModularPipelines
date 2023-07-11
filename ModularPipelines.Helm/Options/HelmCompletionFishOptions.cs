using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "fish")]
public record HelmCompletionFishOptions : HelmOptions
{
    [BooleanCommandSwitch("--no-descriptions")]
    public bool? NoDescriptions { get; set; }

}
