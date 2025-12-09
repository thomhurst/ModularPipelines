using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "traffic-manager", "endpoint", "update")]
public record AzNetworkTrafficManagerEndpointUpdateOptions : AzOptions
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

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--min-child-endpoints")]
    public string? MinChildEndpoints { get; set; }

    [CliOption("--min-child-ipv4")]
    public string? MinChildIpv4 { get; set; }

    [CliOption("--min-child-ipv6")]
    public string? MinChildIpv6 { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--priority")]
    public string? Priority { get; set; }

    [CliOption("--profile-name")]
    public string? ProfileName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subnets")]
    public string? Subnets { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target")]
    public string? Target { get; set; }

    [CliOption("--target-resource-id")]
    public string? TargetResourceId { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--weight")]
    public string? Weight { get; set; }
}