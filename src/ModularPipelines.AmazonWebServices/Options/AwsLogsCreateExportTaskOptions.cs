using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "create-export-task")]
public record AwsLogsCreateExportTaskOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--from")] long From,
[property: CommandSwitch("--to")] long To,
[property: CommandSwitch("--destination")] string Destination
) : AwsOptions
{
    [CommandSwitch("--task-name")]
    public string? TaskName { get; set; }

    [CommandSwitch("--log-stream-name-prefix")]
    public string? LogStreamNamePrefix { get; set; }

    [CommandSwitch("--destination-prefix")]
    public string? DestinationPrefix { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}