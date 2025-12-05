using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "get-users")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigGetUsersOptions : KubernetesOptions
{
    [CliFlag("--set-raw-bytes")]
    public virtual bool? SetRawBytes { get; set; }
}