using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "describe-spot-fleet-request-history")]
public record AwsEc2DescribeSpotFleetRequestHistoryOptions(
[property: CommandSwitch("--spot-fleet-request-id")] string SpotFleetRequestId,
[property: CommandSwitch("--start-time")] long StartTime
) : AwsOptions
{
    [CommandSwitch("--event-type")]
    public string? EventType { get; set; }

    [CommandSwitch("--max-results")]
    public int? MaxResults { get; set; }

    [CommandSwitch("--next-token")]
    public string? NextToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}