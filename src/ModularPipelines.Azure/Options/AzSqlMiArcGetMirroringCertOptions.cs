using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sql", "mi-arc", "get-mirroring-cert")]
public record AzSqlMiArcGetMirroringCertOptions(
[property: CliOption("--cert-file")] string CertFile,
[property: CliOption("--name")] string Name
) : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}