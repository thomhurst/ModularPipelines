using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "get-activity-task")]
public record AwsStepfunctionsGetActivityTaskOptions(
[property: CommandSwitch("--activity-arn")] string ActivityArn
) : AwsOptions
{
    [CommandSwitch("--worker-name")]
    public string? WorkerName { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}