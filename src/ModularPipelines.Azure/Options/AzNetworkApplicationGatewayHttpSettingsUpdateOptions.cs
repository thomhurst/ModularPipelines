using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "application-gateway", "http-settings", "update")]
public record AzNetworkApplicationGatewayHttpSettingsUpdateOptions(
[property: CommandSwitch("--gateway-name")] string GatewayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--affinity-cookie-name")]
    public string? AffinityCookieName { get; set; }

    [CommandSwitch("--auth-certs")]
    public string? AuthCerts { get; set; }

    [CommandSwitch("--connection-draining-timeout")]
    public string? ConnectionDrainingTimeout { get; set; }

    [CommandSwitch("--cookie-based-affinity")]
    public string? CookieBasedAffinity { get; set; }

    [BooleanCommandSwitch("--enable-probe")]
    public bool? EnableProbe { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--host-name")]
    public string? HostName { get; set; }

    [BooleanCommandSwitch("--host-name-from-backend-pool")]
    public bool? HostNameFromBackendPool { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [CommandSwitch("--port")]
    public int? Port { get; set; }

    [CommandSwitch("--probe")]
    public string? Probe { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--root-certs")]
    public string? RootCerts { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}