using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "get-clusters")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigGetClustersOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CommandEqualsSeparatorSwitch("--output", SwitchValueSeparator = " ")]
    public string? Output { get; set; }
}