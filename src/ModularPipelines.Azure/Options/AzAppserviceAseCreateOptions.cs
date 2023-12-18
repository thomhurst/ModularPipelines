using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appservice", "ase", "create")]
public record AzAppserviceAseCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [BooleanCommandSwitch("--force-network-security-group")]
    public bool? ForceNetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--force-route-table")]
    public bool? ForceRouteTable { get; set; }

    [CommandSwitch("--front-end-scale-factor")]
    public string? FrontEndScaleFactor { get; set; }

    [CommandSwitch("--front-end-sku")]
    public string? FrontEndSku { get; set; }

    [BooleanCommandSwitch("--ignore-network-security-group")]
    public bool? IgnoreNetworkSecurityGroup { get; set; }

    [BooleanCommandSwitch("--ignore-route-table")]
    public bool? IgnoreRouteTable { get; set; }

    [BooleanCommandSwitch("--ignore-subnet-size-validation")]
    public bool? IgnoreSubnetSizeValidation { get; set; }

    [CommandSwitch("--kind")]
    public string? Kind { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-preference")]
    public string? OsPreference { get; set; }

    [CommandSwitch("--virtual-ip-type")]
    public string? VirtualIpType { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }

    [BooleanCommandSwitch("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}