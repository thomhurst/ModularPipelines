using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "create-transit-gateway-vpc-attachment")]
public record AwsEc2CreateTransitGatewayVpcAttachmentOptions(
[property: CommandSwitch("--transit-gateway-id")] string TransitGatewayId,
[property: CommandSwitch("--vpc-id")] string VpcId,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--options")]
    public string? Options { get; set; }

    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}