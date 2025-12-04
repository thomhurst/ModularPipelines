using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "instance-failover-group-arc", "create")]
public record AzSqlInstanceFailoverGroupArcCreateOptions(
[property: CliOption("--mi")] string Mi,
[property: CliOption("--name")] string Name,
[property: CliOption("--partner-mi")] string PartnerMi
) : AzOptions
{
    [CliOption("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--partner-mirroring-cert-file")]
    public string? PartnerMirroringCertFile { get; set; }

    [CliOption("--partner-mirroring-url")]
    public string? PartnerMirroringUrl { get; set; }

    [CliOption("--partner-resource-group")]
    public string? PartnerResourceGroup { get; set; }

    [CliOption("--partner-sync-mode")]
    public string? PartnerSyncMode { get; set; }

    [CliOption("--primary-mirroring-url")]
    public string? PrimaryMirroringUrl { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--role")]
    public string? Role { get; set; }

    [CliOption("--shared-name")]
    public string? SharedName { get; set; }

    [CliFlag("--use-k8s")]
    public bool? UseK8s { get; set; }
}