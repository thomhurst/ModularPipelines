using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "server-arc", "endpoint", "list")]
public record AzPostgresServerArcEndpointListOptions : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}