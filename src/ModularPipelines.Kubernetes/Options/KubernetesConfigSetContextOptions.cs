using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CommandPrecedingArguments("config", "set-context")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetContextOptions([property: PositionalArgument] string Name) : KubernetesOptions
{
    [BooleanCommandSwitch("--current")]
    public virtual bool? Current { get; set; }
}