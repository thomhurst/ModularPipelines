using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "traffic-manager", "endpoint", "create")]
public record AzNetworkTrafficManagerEndpointCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--type")] string Type
) : AzOptions
{
    [CliOption("--always-serve")]
    public string? AlwaysServe { get; set; }

    [CliOption("--custom-headers")]
    public string? CustomHeaders { get; set; }

    [CliOption("--endpoint-location")]
    public string? EndpointLocation { get; set; }

    [CliOption("--endpoint-monitor-status")]
    public string? EndpointMonitorStatus { get; set; }

    [CliOption("--endpoint-status")]
    public string? EndpointStatus { get; set; }

    [CliOption("--geo-mapping")]
    public string? GeoMapping { get; set; }

    [CliOption("--min-child-endpoints")]
    public string? MinChildEndpoints { get; set; }

    [CliOption("--min-child-ipv4")]
    public string? MinChildIpv4 { get; set; }

    [CliOption("--min-child-ipv6")]
    public string? MinChildIpv6 { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--subnets")]
    public string? Subnets { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}