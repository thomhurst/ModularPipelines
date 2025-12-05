using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("swf", "record-activity-task-heartbeat")]
public record AwsSwfRecordActivityTaskHeartbeatOptions(
[property: CliOption("--task-token")] string TaskToken
) : AwsOptions
{
    [CliOption("--details")]
    public string? Details { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}