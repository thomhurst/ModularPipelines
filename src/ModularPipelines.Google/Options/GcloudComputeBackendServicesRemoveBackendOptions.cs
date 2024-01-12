using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "backend-services", "remove-backend")]
public record GcloudComputeBackendServicesRemoveBackendOptions(
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
    [BooleanCommandSwitch("--global")]
    public bool? Global { get; set; }

    [CommandSwitch("--region")]
    public string? Region { get; set; }
}