using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("rollout", "status")]
[ExcludeFromCodeCoverage]
public record KubernetesRolloutStatusOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliOption("--filename")]
    public virtual string[]? Filename { get; set; }

    [CliOption("--kustomize")]
    public virtual string? Kustomize { get; set; }

    [CliFlag("--recursive")]
    public virtual bool? Recursive { get; set; }

    [CliOption("--revision")]
    public virtual int? Revision { get; set; }

    [CliOption("--timeout")]
    public virtual string? Timeout { get; set; }

    [CliFlag("--watch")]
    public virtual bool? Watch { get; set; }
}