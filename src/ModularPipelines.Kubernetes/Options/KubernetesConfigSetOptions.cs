using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "set")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetOptions([property: CliArgument] string Property_name) : KubernetesOptions
{
    [CliFlag("--set-raw-bytes")]
    public virtual bool? SetRawBytes { get; set; }
}