using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetOptions([property: PositionalArgument] string Property_name) : KubernetesOptions
{
    [BooleanCommandSwitch("--set-raw-bytes")]
    public bool? SetRawBytes { get; set; }
}