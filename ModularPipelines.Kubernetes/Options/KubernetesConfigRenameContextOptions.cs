using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "rename-context")]
public record KubernetesConfigRenameContextOptions([property: PositionalArgument] string ContextName) : KubernetesOptions
{
    [BooleanCommandSwitch("set-raw-bytes")]
    public bool? SetRawBytes { get; set; }

}
