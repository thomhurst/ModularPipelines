using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("aks", "addon", "update")]
public record AzAksAddonUpdateOptions(
[property: CliOption("--addon")] string Addon,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--appgw-id")]
    public string? AppgwId { get; set; }

    [CliOption("--appgw-name")]
    public string? AppgwName { get; set; }

    [CliOption("--appgw-subnet-cidr")]
    public string? AppgwSubnetCidr { get; set; }

    [CliOption("--appgw-subnet-id")]
    public string? AppgwSubnetId { get; set; }

    [CliOption("--appgw-watch-namespace")]
    public string? AppgwWatchNamespace { get; set; }

    [CliOption("--data-collection-settings")]
    public string? DataCollectionSettings { get; set; }

    [CliOption("--dns-zone-resource-ids")]
    public string? DnsZoneResourceIds { get; set; }

    [CliFlag("--enable-msi-auth-for-monitoring")]
    public bool? EnableMsiAuthForMonitoring { get; set; }

    [CliFlag("--enable-secret-rotation")]
    public bool? EnableSecretRotation { get; set; }

    [CliFlag("--enable-sgxquotehelper")]
    public bool? EnableSgxquotehelper { get; set; }

    [CliFlag("--enable-syslog")]
    public bool? EnableSyslog { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--rotation-poll-interval")]
    public string? RotationPollInterval { get; set; }

    [CliOption("--subnet-name")]
    public string? SubnetName { get; set; }

    [CliOption("--workspace-resource-id")]
    public string? WorkspaceResourceId { get; set; }
}