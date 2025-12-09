using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "purchase-capacity-block")]
public record AwsEc2PurchaseCapacityBlockOptions(
[property: CliOption("--capacity-block-offering-id")] string CapacityBlockOfferingId,
[property: CliOption("--instance-platform")] string InstancePlatform
) : AwsOptions
{
    [CliOption("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}