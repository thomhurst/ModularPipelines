using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "front-door", "backend-pool", "create")]
public record AzNetworkFrontDoorBackendPoolCreateOptions(
[property: CommandSwitch("--address")] string Address,
[property: CommandSwitch("--front-door-name")] string FrontDoorName,
[property: CommandSwitch("--load-balancing")] string LoadBalancing,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--probe")] string Probe,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--backend-host-header")]
    public string? BackendHostHeader { get; set; }

    [BooleanCommandSwitch("--disabled")]
    public bool? Disabled { get; set; }

    [CommandSwitch("--http-port")]
    public string? HttpPort { get; set; }

    [CommandSwitch("--https-port")]
    public string? HttpsPort { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}