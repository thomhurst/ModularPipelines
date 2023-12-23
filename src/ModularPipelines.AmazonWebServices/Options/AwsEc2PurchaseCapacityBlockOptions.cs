using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "purchase-capacity-block")]
public record AwsEc2PurchaseCapacityBlockOptions(
[property: CommandSwitch("--capacity-block-offering-id")] string CapacityBlockOfferingId,
[property: CommandSwitch("--instance-platform")] string InstancePlatform
) : AwsOptions
{
    [CommandSwitch("--tag-specifications")]
    public string[]? TagSpecifications { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}