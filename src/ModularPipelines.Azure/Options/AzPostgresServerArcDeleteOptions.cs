using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "server-arc", "delete")]
public record AzPostgresServerArcDeleteOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}