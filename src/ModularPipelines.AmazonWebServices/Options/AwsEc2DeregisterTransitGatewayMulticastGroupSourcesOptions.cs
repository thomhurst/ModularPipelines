using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "deregister-transit-gateway-multicast-group-sources")]
public record AwsEc2DeregisterTransitGatewayMulticastGroupSourcesOptions : AwsOptions
{
    [CliOption("--transit-gateway-multicast-domain-id")]
    public string? TransitGatewayMulticastDomainId { get; set; }

    [CliOption("--group-ip-address")]
    public string? GroupIpAddress { get; set; }

    [CliOption("--network-interface-ids")]
    public string[]? NetworkInterfaceIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}