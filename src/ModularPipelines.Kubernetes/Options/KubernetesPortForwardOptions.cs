using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("port-forward")]
[ExcludeFromCodeCoverage]
public record KubernetesPortForwardOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--address")]
    public virtual string? Address { get; set; }

    [CliOption("--pod-running-timeout")]
    public virtual string? PodRunningTimeout { get; set; }
}