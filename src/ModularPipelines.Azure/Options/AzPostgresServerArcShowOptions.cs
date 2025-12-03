using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "server-arc", "show")]
public record AzPostgresServerArcShowOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}