using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "listener", "create")]
public record AzNetworkApplicationGatewayListenerCreateOptions(
[property: CommandSwitch("--frontend-port")] string FrontendPort,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CommandSwitch("--host-names")]
    public string? HostNames { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ssl-cert")]
    public string? SslCert { get; set; }

    [CommandSwitch("--ssl-profile-id")]
    public string? SslProfileId { get; set; }
}