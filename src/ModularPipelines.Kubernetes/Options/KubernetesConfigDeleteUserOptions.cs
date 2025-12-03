using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Kubernetes.Options;

[CliCommand("config", "delete-user")]
[ExcludeFromCodeCoverage]
public record KubernetesConfigDeleteUserOptions([property: CliArgument] string Name) : KubernetesOptions
{
    [CliFlag("--no-headers")]
    public virtual bool? NoHeaders { get; set; }

    [CliOption("--output")]
    public string? Output { get; set; }
}