using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "mi-arc", "get-mirroring-cert")]
public record AzSqlMiArcGetMirroringCertOptions(
[property: CommandSwitch("--cert-file")] string CertFile,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}