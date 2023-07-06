using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set-context")]
public record KubernetesConfigSetContextOptions(string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("current")]
    public bool? Current { get; set; }

}
