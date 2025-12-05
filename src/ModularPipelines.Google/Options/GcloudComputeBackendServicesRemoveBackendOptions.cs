using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "backend-services", "remove-backend")]
public record GcloudComputeBackendServicesRemoveBackendOptions(
[property: CliArgument] string BackendServiceName,
[property: CliOption("--instance-group")] string InstanceGroup,
[property: CliOption("--instance-group-region")] string InstanceGroupRegion,
[property: CliOption("--instance-group-zone")] string InstanceGroupZone,
[property: CliOption("--network-endpoint-group")] string NetworkEndpointGroup,
[property: CliFlag("--global-network-endpoint-group")] bool GlobalNetworkEndpointGroup,
[property: CliOption("--network-endpoint-group-region")] string NetworkEndpointGroupRegion,
[property: CliOption("--network-endpoint-group-zone")] string NetworkEndpointGroupZone
) : GcloudOptions
{
    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--region")]
    public string? Region { get; set; }
}