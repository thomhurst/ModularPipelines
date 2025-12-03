using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("port-forward")]
[ExcludeFromCodeCoverage]
public record KubernetesPortForwardOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--address")]
    public string? Address { get; set; }

    [CliOption("--pod-running-timeout")]
    public string? PodRunningTimeout { get; set; }
}