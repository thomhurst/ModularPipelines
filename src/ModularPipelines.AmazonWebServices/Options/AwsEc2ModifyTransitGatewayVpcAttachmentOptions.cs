using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "modify-transit-gateway-vpc-attachment")]
public record AwsEc2ModifyTransitGatewayVpcAttachmentOptions(
[property: CliOption("--transit-gateway-attachment-id")] string TransitGatewayAttachmentId
) : AwsOptions
{
    [CliOption("--add-subnet-ids")]
    public string[]? AddSubnetIds { get; set; }

    [CliOption("--remove-subnet-ids")]
    public string[]? RemoveSubnetIds { get; set; }

    [CliOption("--options")]
    public string? Options { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}