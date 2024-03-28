using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesCpOptions : KubernetesOptions
{
    public KubernetesCpOptions()
    {
        CommandParts = ["cp"];
    }

    [CommandSwitch("--container")]
    public string? Container { get; set; }

    [BooleanCommandSwitch("--no-preserve")]
    public bool? NoPreserve { get; set; }
}
