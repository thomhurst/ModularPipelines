using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "register-transit-gateway-multicast-group-members")]
public record AwsEc2RegisterTransitGatewayMulticastGroupMembersOptions(
[property: CliOption("--transit-gateway-multicast-domain-id")] string TransitGatewayMulticastDomainId,
[property: CliOption("--network-interface-ids")] string[] NetworkInterfaceIds
) : AwsOptions
{
    [CliOption("--group-ip-address")]
    public string? GroupIpAddress { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}