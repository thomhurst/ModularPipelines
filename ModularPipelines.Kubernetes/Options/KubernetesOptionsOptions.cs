using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("options")]
public record KubernetesOptionsOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--name-only")]
    public bool? NameOnly { get; set; }
}
