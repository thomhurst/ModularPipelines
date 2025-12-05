using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logs", "create-export-task")]
public record AwsLogsCreateExportTaskOptions(
[property: CliOption("--log-group-name")] string LogGroupName,
[property: CliOption("--from")] long From,
[property: CliOption("--to")] long To,
[property: CliOption("--destination")] string Destination
) : AwsOptions
{
    [CliOption("--task-name")]
    public string? TaskName { get; set; }

    [CliOption("--log-stream-name-prefix")]
    public string? LogStreamNamePrefix { get; set; }

    [CliOption("--destination-prefix")]
    public string? DestinationPrefix { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}