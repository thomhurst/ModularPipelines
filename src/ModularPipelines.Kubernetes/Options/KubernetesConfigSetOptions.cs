using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigSetOptions : KubernetesOptions
{
    public KubernetesConfigSetOptions()
    {
        CommandParts = ["config", "set"];
    }

    [BooleanCommandSwitch("--set-raw-bytes")]
    public bool? SetRawBytes { get; set; }
}
