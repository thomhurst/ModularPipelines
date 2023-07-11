using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("plugin")]
public record KubernetesPluginOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }

}
