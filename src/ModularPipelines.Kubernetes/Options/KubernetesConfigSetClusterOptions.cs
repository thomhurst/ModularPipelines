using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "set-cluster")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetClusterOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--embed-certs")]
    public virtual bool? EmbedCerts { get; set; }
}