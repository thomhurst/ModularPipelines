using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "reprovision-replica")]
public record AzSqlMiArcReprovisionReplicaOptions(
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}