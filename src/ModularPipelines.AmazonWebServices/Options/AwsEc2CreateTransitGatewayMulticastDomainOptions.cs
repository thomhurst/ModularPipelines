using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-multicast-domain")]
public record AwsEc2CreateTransitGatewayMulticastDomainOptions(
[property: CommandSwitch("--transit-gateway-id")] string TransitGatewayId
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}