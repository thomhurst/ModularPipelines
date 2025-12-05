using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-transit-gateway-vpc-attachment")]
public record AwsEc2CreateTransitGatewayVpcAttachmentOptions(
[property: CliOption("--transit-gateway-id")] string TransitGatewayId,
[property: CliOption("--vpc-id")] string VpcId,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}