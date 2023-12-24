using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-transit-gateway-multicast-domain")]
public record AwsEc2DeleteTransitGatewayMulticastDomainOptions(
[property: CommandSwitch("--transit-gateway-multicast-domain-id")] string TransitGatewayMulticastDomainId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}