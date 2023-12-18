using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "instance-failover-group-arc", "create")]
public record AzSqlInstanceFailoverGroupArcCreateOptions(
[property: CommandSwitch("--mi")] string Mi,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--partner-mi")] string PartnerMi
) : AzOptions
{
    [CommandSwitch("--k8s-namespace")]
    public string? K8sNamespace { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--partner-mirroring-cert-file")]
    public string? PartnerMirroringCertFile { get; set; }

    [CommandSwitch("--partner-mirroring-url")]
    public string? PartnerMirroringUrl { get; set; }

    [CommandSwitch("--partner-resource-group")]
    public string? PartnerResourceGroup { get; set; }

    [CommandSwitch("--partner-sync-mode")]
    public string? PartnerSyncMode { get; set; }

    [CommandSwitch("--primary-mirroring-url")]
    public string? PrimaryMirroringUrl { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--role")]
    public string? Role { get; set; }

    [CommandSwitch("--shared-name")]
    public string? SharedName { get; set; }

    [BooleanCommandSwitch("--use-k8s")]
    public bool? UseK8s { get; set; }
}