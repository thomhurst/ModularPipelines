using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("version")]
[ExcludeFromCodeCoverage]
public record KubernetesVersionOptions : KubernetesOptions
{
    [CliFlag("--client")]
    public virtual bool? Client { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }

    [CliFlag("--short")]
    public virtual bool? Short { get; set; }
}