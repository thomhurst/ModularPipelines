using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "get-log-events")]
public record AwsLogsGetLogEventsOptions(
[property: CliOption("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CliOption("--log-group-name")]
    public string? LogGroupName { get; set; }

    [CliOption("--log-group-identifier")]
    public string? LogGroupIdentifier { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }

    [CliOption("--limit")]
    public int? Limit { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}