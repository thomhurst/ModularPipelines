using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("aks", "addon", "enable")]
public record AzAksAddonEnableOptions(
[property: CommandSwitch("--addon")] string Addon,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--appgw-id")]
    public string? AppgwId { get; set; }

    [CommandSwitch("--appgw-name")]
    public string? AppgwName { get; set; }

    [CommandSwitch("--appgw-subnet-cidr")]
    public string? AppgwSubnetCidr { get; set; }

    [CommandSwitch("--appgw-subnet-id")]
    public string? AppgwSubnetId { get; set; }

    [CommandSwitch("--appgw-watch-namespace")]
    public string? AppgwWatchNamespace { get; set; }

    [CommandSwitch("--data-collection-settings")]
    public string? DataCollectionSettings { get; set; }

    [CommandSwitch("--dns-zone-resource-ids")]
    public string? DnsZoneResourceIds { get; set; }

    [BooleanCommandSwitch("--enable-msi-auth-for-monitoring")]
    public bool? EnableMsiAuthForMonitoring { get; set; }

    [BooleanCommandSwitch("--enable-secret-rotation")]
    public bool? EnableSecretRotation { get; set; }

    [BooleanCommandSwitch("--enable-sgxquotehelper")]
    public bool? EnableSgxquotehelper { get; set; }

    [BooleanCommandSwitch("--enable-syslog")]
    public bool? EnableSyslog { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--rotation-poll-interval")]
    public string? RotationPollInterval { get; set; }

    [CommandSwitch("--subnet-name")]
    public string? SubnetName { get; set; }

    [CommandSwitch("--workspace-resource-id")]
    public string? WorkspaceResourceId { get; set; }
}