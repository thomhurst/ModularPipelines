using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "private-link-service", "create")]
public record AzNetworkPrivateLinkServiceCreateOptions(
[property: CliOption("--lb-frontend-ip-configs")] string LbFrontendIpConfigs,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--auto-approval")]
    public string? AutoApproval { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliFlag("--enable-proxy-protocol")]
    public bool? EnableProxyProtocol { get; set; }

    [CliOption("--fqdns")]
    public string? Fqdns { get; set; }

    [CliOption("--lb-name")]
    public string? LbName { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-ip-address")]
    public string? PrivateIpAddress { get; set; }

    [CliOption("--private-ip-address-version")]
    public string? PrivateIpAddressVersion { get; set; }

    [CliOption("--private-ip-allocation-method")]
    public string? PrivateIpAllocationMethod { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--visibility")]
    public string? Visibility { get; set; }

    [CliOption("--vnet-name")]
    public string? VnetName { get; set; }
}