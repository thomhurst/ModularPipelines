using ModularPipelines.Attributes;

namespace ModularPipelines.Helm.Options;

[CommandPrecedingArguments("completion", "bash")]
public record HelmCompletionBashOptions : HelmOptions
{
    [BooleanCommandSwitch("no-descriptions")]
    public bool? NoDescriptions { get; set; }

}
