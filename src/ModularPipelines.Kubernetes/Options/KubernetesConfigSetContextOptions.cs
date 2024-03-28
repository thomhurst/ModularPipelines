using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[ExcludeFromCodeCoverage]
public record KubernetesConfigSetContextOptions : KubernetesOptions
{
    public KubernetesConfigSetContextOptions()
    {
        CommandParts = ["config", "set-context"];
    }

    [CommandSwitch("--cluster")]
    public string? Cluster { get; set; }

    [BooleanCommandSwitch("--current")]
    public bool? Current { get; set; }

    [PositionalArgument(PlaceholderName = "<NameCurrent>")]
    public string? NameCurrent { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--user")]
    public string? User { get; set; }
}
