using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("logs", "create-log-stream")]
public record AwsLogsCreateLogStreamOptions(
[property: CommandSwitch("--log-group-name")] string LogGroupName,
[property: CommandSwitch("--log-stream-name")] string LogStreamName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}