using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-reserved-instances-listings")]
public record AwsEc2DescribeReservedInstancesListingsOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--reserved-instances-id")]
    public string? ReservedInstancesId { get; set; }

    [CliOption("--reserved-instances-listing-id")]
    public string? ReservedInstancesListingId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}