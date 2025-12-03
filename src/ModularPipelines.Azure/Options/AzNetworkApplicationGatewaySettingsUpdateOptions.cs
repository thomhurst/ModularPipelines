using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "application-gateway", "settings", "update")]
public record AzNetworkApplicationGatewaySettingsUpdateOptions(
[property: CliOption("--gateway-name")] string GatewayName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--backend-pool-host-name")]
    public bool? BackendPoolHostName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--host-name")]
    public string? HostName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--port")]
    public int? Port { get; set; }

    [CliOption("--probe")]
    public string? Probe { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--root-certs")]
    public string? RootCerts { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}