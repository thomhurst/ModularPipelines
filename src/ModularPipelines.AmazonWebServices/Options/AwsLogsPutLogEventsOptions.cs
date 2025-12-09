using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "put-log-events")]
public record AwsLogsPutLogEventsOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--log-stream-name")] string LogStreamName,
[property: CliOption("--log-events")] string[] LogEvents
) : AwsOptions
{
    [CliOption("--sequence-token")]
    public string? SequenceToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}