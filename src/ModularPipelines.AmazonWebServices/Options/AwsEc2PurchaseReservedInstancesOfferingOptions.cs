using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "purchase-reserved-instances-offering")]
public record AwsEc2PurchaseReservedInstancesOfferingOptions(
[property: CliOption("--instance-count")] int InstanceCount,
[property: CliOption("--reserved-instances-offering-id")] string ReservedInstancesOfferingId
) : AwsOptions
{
    [CliOption("--limit-price")]
    public string? LimitPrice { get; set; }

    [CliOption("--purchase-time")]
    public long? PurchaseTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}