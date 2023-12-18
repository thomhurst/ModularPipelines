using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "private-link-service", "create")]
public record AzNetworkPrivateLinkServiceCreateOptions(
[property: CommandSwitch("--lb-frontend-ip-configs")] string LbFrontendIpConfigs,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--auto-approval")]
    public string? AutoApproval { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [BooleanCommandSwitch("--enable-proxy-protocol")]
    public bool? EnableProxyProtocol { get; set; }

    [CommandSwitch("--fqdns")]
    public string? Fqdns { get; set; }

    [CommandSwitch("--lb-name")]
    public string? LbName { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CommandSwitch("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CommandSwitch("--private-ip-allocation-method")]
    public string? PrivateIpAllocationMethod { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--visibility")]
    public string? Visibility { get; set; }

    [CommandSwitch("--vnet-name")]
    public string? VnetName { get; set; }
}

