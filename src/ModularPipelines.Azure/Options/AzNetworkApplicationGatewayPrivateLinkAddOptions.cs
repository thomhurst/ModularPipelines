using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "private-link", "add")]
public record AzNetworkApplicationGatewayPrivateLinkAddOptions(
[property: CommandSwitch("--frontend-ip")] string FrontendIp,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--subnet")] string Subnet
) : AzOptions
{
    [CommandSwitch("--ip-address")]
    public string? IpAddress { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [BooleanCommandSwitch("--primary")]
    public bool? Primary { get; set; }

    [CommandSwitch("--subnet-prefix")]
    public string? SubnetPrefix { get; set; }
}