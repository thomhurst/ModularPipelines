using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "poll-for-activity-task")]
public record AwsSwfPollForActivityTaskOptions(
[property: CommandSwitch("--domain")] string Domain,
[property: CommandSwitch("--task-list")] string TaskList
) : AwsOptions
{
    [CommandSwitch("--identity")]
    public string? Identity { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}