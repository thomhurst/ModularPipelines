using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "register-transit-gateway-multicast-group-members")]
public record AwsEc2RegisterTransitGatewayMulticastGroupMembersOptions(
[property: CommandSwitch("--transit-gateway-multicast-domain-id")] string TransitGatewayMulticastDomainId,
[property: CommandSwitch("--network-interface-ids")] string[] NetworkInterfaceIds
) : AwsOptions
{
    [CommandSwitch("--group-ip-address")]
    public string? GroupIpAddress { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}