using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-capacity-block-offerings")]
public record AwsEc2DescribeCapacityBlockOfferingsOptions(
[property: CliOption("--instance-type")] string InstanceType,
[property: CliOption("--instance-count")] int InstanceCount,
[property: CliOption("--capacity-duration-hours")] int CapacityDurationHours
) : AwsOptions
{
    [CliOption("--start-date-range")]
    public long? StartDateRange { get; set; }

    [CliOption("--end-date-range")]
    public long? EndDateRange { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}