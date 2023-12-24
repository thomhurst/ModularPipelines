using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "purchase-reserved-instances-offering")]
public record AwsEc2PurchaseReservedInstancesOfferingOptions(
[property: CommandSwitch("--instance-count")] int InstanceCount,
[property: CommandSwitch("--reserved-instances-offering-id")] string ReservedInstancesOfferingId
) : AwsOptions
{
    [CommandSwitch("--limit-price")]
    public string? LimitPrice { get; set; }

    [CommandSwitch("--purchase-time")]
    public long? PurchaseTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}