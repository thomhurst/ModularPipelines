using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "delete-log-stream")]
public record AwsLogsDeleteLogStreamOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}