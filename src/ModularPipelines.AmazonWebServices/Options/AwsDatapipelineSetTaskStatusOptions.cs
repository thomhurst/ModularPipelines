using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "set-task-status")]
public record AwsDatapipelineSetTaskStatusOptions(
[property: CommandSwitch("--task-id")] string TaskId,
[property: CommandSwitch("--task-status")] string TaskStatus
) : AwsOptions
{
    [CommandSwitch("--error-id")]
    public string? ErrorId { get; set; }

    [CommandSwitch("--error-message")]
    public string? ErrorMessage { get; set; }

    [CommandSwitch("--error-stack-trace")]
    public string? ErrorStackTrace { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}