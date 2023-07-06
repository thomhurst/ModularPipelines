using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "rename-context")]
public record KubernetesConfigRenameContextOptions(string Context_name) : KubernetesOptions
{
    [BooleanCommandSwitch("set-raw-bytes")]
    public bool? SetRawBytes { get; set; }

}
