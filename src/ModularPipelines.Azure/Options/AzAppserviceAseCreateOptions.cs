using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appservice", "ase", "create")]
public record AzAppserviceAseCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliFlag("--force-network-security-group")]
    public bool? ForceNetworkSecurityGroup { get; set; }

    [CliFlag("--force-route-table")]
    public bool? ForceRouteTable { get; set; }

    [CliOption("--front-end-scale-factor")]
    public string? FrontEndScaleFactor { get; set; }

    [CliOption("--front-end-sku")]
    public string? FrontEndSku { get; set; }

    [CliFlag("--ignore-network-security-group")]
    public bool? IgnoreNetworkSecurityGroup { get; set; }

    [CliFlag("--ignore-route-table")]
    public bool? IgnoreRouteTable { get; set; }

    [CliFlag("--ignore-subnet-size-validation")]
    public bool? IgnoreSubnetSizeValidation { get; set; }

    [CliOption("--kind")]
    public string? Kind { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-preference")]
    public string? OsPreference { get; set; }

    [CliOption("--virtual-ip-type")]
    public string? VirtualIpType { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }

    [CliFlag("--zone-redundant")]
    public bool? ZoneRedundant { get; set; }
}