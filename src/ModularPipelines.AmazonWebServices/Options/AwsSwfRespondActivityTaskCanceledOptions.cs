using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("swf", "respond-activity-task-canceled")]
public record AwsSwfRespondActivityTaskCanceledOptions(
[property: CommandSwitch("--task-token")] string TaskToken
) : AwsOptions
{
    [CommandSwitch("--details")]
    public string? Details { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}