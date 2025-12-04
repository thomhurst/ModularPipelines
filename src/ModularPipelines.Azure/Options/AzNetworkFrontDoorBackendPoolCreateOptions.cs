using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "front-door", "backend-pool", "create")]
public record AzNetworkFrontDoorBackendPoolCreateOptions(
[property: CliOption("--address")] string Address,
[property: CliOption("--front-door-name")] string FrontDoorName,
[property: CliOption("--load-balancing")] string LoadBalancing,
[property: CliOption("--name")] string Name,
[property: CliOption("--probe")] string Probe,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--backend-host-header")]
    public string? BackendHostHeader { get; set; }

    [CliFlag("--disabled")]
    public bool? Disabled { get; set; }

    [CliOption("--http-port")]
    public string? HttpPort { get; set; }

    [CliOption("--https-port")]
    public string? HttpsPort { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}