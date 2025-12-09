using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("postgres", "server-arc", "list")]
public record AzPostgresServerArcListOptions : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}