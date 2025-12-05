using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-fleet-history")]
public record AwsEc2DescribeFleetHistoryOptions(
[property: CliOption("--fleet-id")] string FleetId,
[property: CliOption("--start-time")] long StartTime
) : AwsOptions
{
    [CliOption("--event-type")]
    public string? EventType { get; set; }

    [CliOption("--max-results")]
    public int? MaxResults { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}