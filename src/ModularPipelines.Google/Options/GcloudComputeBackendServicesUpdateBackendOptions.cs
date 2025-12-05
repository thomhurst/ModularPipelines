using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "update-backend")]
public record GcloudComputeBackendServicesUpdateBackendOptions(
[property: CliArgument] string BackendServiceName,
[property: CliOption("--instance-group")] string InstanceGroup,
[property: CliOption("--instance-group-region")] string InstanceGroupRegion,
[property: CliOption("--instance-group-zone")] string InstanceGroupZone,
[property: CliOption("--network-endpoint-group")] string NetworkEndpointGroup,
[property: CliOption("--network-endpoint-group-zone")] string NetworkEndpointGroupZone
) : GcloudOptions
{
    [CliOption("--balancing-mode")]
    public string? BalancingMode { get; set; }

    [CliOption("--capacity-scaler")]
    public string? CapacityScaler { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliFlag("--failover")]
    public bool? Failover { get; set; }

    [CliOption("--max-utilization")]
    public string? MaxUtilization { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }

    [CliOption("--max-connections")]
    public string? MaxConnections { get; set; }

    [CliOption("--max-connections-per-endpoint")]
    public string? MaxConnectionsPerEndpoint { get; set; }

    [CliOption("--max-connections-per-instance")]
    public string? MaxConnectionsPerInstance { get; set; }

    [CliOption("--max-rate")]
    public string? MaxRate { get; set; }

    [CliOption("--max-rate-per-endpoint")]
    public string? MaxRatePerEndpoint { get; set; }

    [CliOption("--max-rate-per-instance")]
    public string? MaxRatePerInstance { get; set; }
}