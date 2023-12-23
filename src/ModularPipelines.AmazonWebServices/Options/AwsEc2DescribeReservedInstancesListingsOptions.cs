using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-reserved-instances-listings")]
public record AwsEc2DescribeReservedInstancesListingsOptions : AwsOptions
{
    [CommandSwitch("--filters")]
    public string[]? Filters { get; set; }

    [CommandSwitch("--reserved-instances-id")]
    public string? ReservedInstancesId { get; set; }

    [CommandSwitch("--reserved-instances-listing-id")]
    public string? ReservedInstancesListingId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}