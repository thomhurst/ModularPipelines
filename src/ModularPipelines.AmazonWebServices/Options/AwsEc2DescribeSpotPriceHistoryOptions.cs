using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-spot-price-history")]
public record AwsEc2DescribeSpotPriceHistoryOptions : AwsOptions
{
    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--instance-types")]
    public string[]? InstanceTypes { get; set; }

    [CliOption("--product-descriptions")]
    public string[]? ProductDescriptions { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}