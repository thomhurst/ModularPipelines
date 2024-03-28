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

    [PositionalArgument(PlaceholderName = "LocalPORT:")]
    public string? LocalPORT: { get; set;
}

[PositionalArgument(PlaceholderName = "LocalPortN:RemotePortN")]
public string? LocalPortN:RemotePortN { get; set; }

[CommandSwitch("--pod-running-timeout")]
public string? PodRunningTimeout { get; set; }
}
