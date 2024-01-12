using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "add-backend")]
public record GcloudComputeBackendServicesAddBackendOptions(
[property: PositionalArgument] string BackendServiceName,
[property: CommandSwitch("--instance-group")] string InstanceGroup,
[property: CommandSwitch("--instance-group-region")] string InstanceGroupRegion,
[property: CommandSwitch("--instance-group-zone")] string InstanceGroupZone,
[property: CommandSwitch("--network-endpoint-group")] string NetworkEndpointGroup,
[property: BooleanCommandSwitch("--global-network-endpoint-group")] bool GlobalNetworkEndpointGroup,
[property: CommandSwitch("--network-endpoint-group-region")] string NetworkEndpointGroupRegion,
[property: CommandSwitch("--network-endpoint-group-zone")] string NetworkEndpointGroupZone
) : GcloudOptions
{
    [CommandSwitch("--balancing-mode")]
    public string? BalancingMode { get; set; }

    [CommandSwitch("--capacity-scaler")]
    public string? CapacityScaler { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [BooleanCommandSwitch("--failover")]
    public bool? Failover { get; set; }

    [CommandSwitch("--max-utilization")]
    public string? MaxUtilization { get; set; }

    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }

    [CommandSwitch("--max-connections")]
    public string? MaxConnections { get; set; }

    [CommandSwitch("--max-connections-per-endpoint")]
    public string? MaxConnectionsPerEndpoint { get; set; }

    [CommandSwitch("--max-connections-per-instance")]
    public string? MaxConnectionsPerInstance { get; set; }

    [CommandSwitch("--max-rate")]
    public string? MaxRate { get; set; }

    [CommandSwitch("--max-rate-per-endpoint")]
    public string? MaxRatePerEndpoint { get; set; }

    [CommandSwitch("--max-rate-per-instance")]
    public string? MaxRatePerInstance { get; set; }
}