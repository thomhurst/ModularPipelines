using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("port-forward")]
public record KubernetesPortForwardOptions(string Name) : KubernetesOptions
{
    [CommandLongSwitch("address", SwitchValueSeparator = " ")]
    public string? Address { get; set; }

    [CommandLongSwitch("pod-running-timeout", SwitchValueSeparator = " ")]
    public string? PodRunningTimeout { get; set; }

}
