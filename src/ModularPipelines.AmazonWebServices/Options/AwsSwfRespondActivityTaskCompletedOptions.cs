using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "respond-activity-task-completed")]
public record AwsSwfRespondActivityTaskCompletedOptions(
[property: CommandSwitch("--task-token")] string TaskToken
) : AwsOptions
{
    [CommandSwitch("--result")]
    public string? Result { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}