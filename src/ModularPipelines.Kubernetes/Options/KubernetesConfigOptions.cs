using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliSubCommand("config")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigOptions([property: CliArgument] string Subcommand) : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public virtual string? Output { get; set; }
}