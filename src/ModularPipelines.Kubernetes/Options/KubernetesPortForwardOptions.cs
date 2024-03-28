using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesPortForwardOptions : KubernetesOptions
{
    public KubernetesPortForwardOptions()
    {
        CommandParts = ["port-forward"];
    }

    [CommandSwitch("--address")]
    public string? Address { get; set; }

    [CommandSwitch("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }
}
