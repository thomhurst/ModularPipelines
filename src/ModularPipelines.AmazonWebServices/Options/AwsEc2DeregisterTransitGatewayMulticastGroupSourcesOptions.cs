using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "deregister-transit-gateway-multicast-group-sources")]
public record AwsEc2DeregisterTransitGatewayMulticastGroupSourcesOptions : AwsOptions
{
    [CommandSwitch("--transit-gateway-multicast-domain-id")]
    public string? TransitGatewayMulticastDomainId { get; set; }

    [CommandSwitch("--group-ip-address")]
    public string? GroupIpAddress { get; set; }

    [CommandSwitch("--network-interface-ids")]
    public string[]? NetworkInterfaceIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}