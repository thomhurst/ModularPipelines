using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "http-listener", "create")]
public record AzNetworkApplicationGatewayHttpListenerCreateOptions(
[property: CommandSwitch("--frontend-port")] string FrontendPort,
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--frontend-ip")]
    public string? FrontendIp { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [CommandSwitch("--host-names")]
    public string? HostNames { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--ssl-cert")]
    public string? SslCert { get; set; }

    [CommandSwitch("--ssl-profile-id")]
    public string? SslProfileId { get; set; }

    [CommandSwitch("--waf-policy")]
    public string? WafPolicy { get; set; }
}

