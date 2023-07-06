using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set-cluster")]
public record KubernetesConfigSetClusterOptions(string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("embed-certs")]
    public bool? EmbedCerts { get; set; }

}
