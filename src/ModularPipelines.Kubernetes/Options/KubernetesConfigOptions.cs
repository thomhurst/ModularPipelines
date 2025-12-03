using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigOptions([property: CliArgument] string Subcommand) : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }
}