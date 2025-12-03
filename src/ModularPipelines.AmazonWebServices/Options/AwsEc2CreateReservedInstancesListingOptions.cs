using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "create-reserved-instances-listing")]
public record AwsEc2CreateReservedInstancesListingOptions(
[property: CliOption("--client-token")] string ClientToken,
[property: CliOption("--instance-count")] int InstanceCount,
[property: CliOption("--price-schedules")] string[] PriceSchedules,
[property: CliOption("--reserved-instances-id")] string ReservedInstancesId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}