using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "get-users")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigGetUsersOptions : KubernetesOptions
{
    [BooleanCommandSwitch("--set-raw-bytes")]
    public bool? SetRawBytes { get; set; }
}