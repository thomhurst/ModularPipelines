using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "count-pending-activity-tasks")]
public record AwsSwfCountPendingActivityTasksOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--task-list")] string TaskList
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}