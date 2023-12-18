using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "settings", "create")]
public record AzNetworkApplicationGatewaySettingsCreateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--port")] int Port,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--backend-pool-host-name")]
    public bool? BackendPoolHostName { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--probe")]
    public string? Probe { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--root-certs")]
    public string? RootCerts { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}