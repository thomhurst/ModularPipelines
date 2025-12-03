using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "http-settings", "create")]
public record AzNetworkApplicationGatewayHttpSettingsCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--port")] int Port,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--affinity-cookie-name")]
    public string? AffinityCookieName { get; set; }

    [CliOption("--auth-certs")]
    public string? AuthCerts { get; set; }

    [CliOption("--connection-draining-timeout")]
    public string? ConnectionDrainingTimeout { get; set; }

    [CliOption("--cookie-based-affinity")]
    public string? CookieBasedAffinity { get; set; }

    [CliFlag("--enable-probe")]
    public bool? EnableProbe { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliFlag("--host-name-from-backend-pool")]
    public bool? HostNameFromBackendPool { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliOption("--probe")]
    public string? Probe { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--root-certs")]
    public string? RootCerts { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}