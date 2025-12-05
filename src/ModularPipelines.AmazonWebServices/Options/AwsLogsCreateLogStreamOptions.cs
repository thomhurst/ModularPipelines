using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "create-log-stream")]
public record AwsLogsCreateLogStreamOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}