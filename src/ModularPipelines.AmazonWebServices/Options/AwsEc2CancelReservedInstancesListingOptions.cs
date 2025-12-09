using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "cancel-reserved-instances-listing")]
public record AwsEc2CancelReservedInstancesListingOptions(
[property: CliOption("--reserved-instances-listing-id")] string ReservedInstancesListingId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}