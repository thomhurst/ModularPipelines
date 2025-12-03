using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "set-context")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigSetContextOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--current")]
    public virtual bool? Current { get; set; }
}