using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "traffic-manager", "endpoint", "create")]
public record AzNetworkTrafficManagerEndpointCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--type")] string Type
) : AzOptions
{
    [CommandSwitch("--always-serve")]
    public string? AlwaysServe { get; set; }

    [CommandSwitch("--custom-headers")]
    public string? CustomHeaders { get; set; }

    [CommandSwitch("--endpoint-location")]
    public string? EndpointLocation { get; set; }

    [CommandSwitch("--endpoint-monitor-status")]
    public string? EndpointMonitorStatus { get; set; }

    [CommandSwitch("--endpoint-status")]
    public string? EndpointStatus { get; set; }

    [CommandSwitch("--geo-mapping")]
    public string? GeoMapping { get; set; }

    [CommandSwitch("--min-child-endpoints")]
    public string? MinChildEndpoints { get; set; }

    [CommandSwitch("--min-child-ipv4")]
    public string? MinChildIpv4 { get; set; }

    [CommandSwitch("--min-child-ipv6")]
    public string? MinChildIpv6 { get; set; }

    [CommandSwitch("--priority")]
    public string? Priority { get; set; }

    [CommandSwitch("--subnets")]
    public string? Subnets { get; set; }

    [CommandSwitch("--target")]
    public string? Target { get; set; }

    [CommandSwitch("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CommandSwitch("--weight")]
    public string? Weight { get; set; }
}

