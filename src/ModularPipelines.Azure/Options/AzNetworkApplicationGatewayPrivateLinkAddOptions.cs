using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "application-gateway", "private-link", "add")]
public record AzNetworkApplicationGatewayPrivateLinkAddOptions(
[property: CliOption("--frontend-ip")] string FrontendIp,
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--subnet")] string Subnet
) : AzOptions
{
    [CliOption("--ip-address")]
    public string? IpAddress { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--primary")]
    public bool? Primary { get; set; }

    [CliOption("--subnet-prefix")]
    public string? SubnetPrefix { get; set; }
}