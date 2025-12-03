using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "settings", "create")]
public record AzNetworkApplicationGatewaySettingsCreateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--port")] int Port,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--backend-pool-host-name")]
    public bool? BackendPoolHostName { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--probe")]
    public string? Probe { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--root-certs")]
    public string? RootCerts { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}