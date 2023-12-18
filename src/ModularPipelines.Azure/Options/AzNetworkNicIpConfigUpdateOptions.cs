using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "nic", "ip-config", "update")]
public record AzNetworkNicIpConfigUpdateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--nic-name")] string NicName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--ag-address-pools")]
    public string? AgAddressPools { get; set; }

    [CommandSwitch("--application-security-groups")]
    public string? ApplicationSecurityGroups { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--gateway-lb")]
    public string? GatewayLb { get; set; }

    [CommandSwitch("--gateway-name")]
    public string? GatewayName { get; set; }

    [CommandSwitch("--lb-address-pools")]
    public string? LbAddressPools { get; set; }

    [CommandSwitch("--lb-inbound-nat-rules")]
    public string? LbInboundNatRules { get; set; }

    [CommandSwitch("--lb-name")]
    public string? LbName { get; set; }

    [BooleanCommandSwitch("--make-primary")]
    public bool? MakePrimary { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CommandSwitch("--public-ip-address")]
    public string? PublicIpAddress { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subnet")]
    public string? Subnet { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}