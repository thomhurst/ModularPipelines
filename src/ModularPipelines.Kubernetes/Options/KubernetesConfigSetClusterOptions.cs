using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set-cluster")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetClusterOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--embed-certs")]
    public bool? EmbedCerts { get; set; }
}