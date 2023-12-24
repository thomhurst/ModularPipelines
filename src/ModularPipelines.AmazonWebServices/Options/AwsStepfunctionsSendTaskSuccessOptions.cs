using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "send-task-success")]
public record AwsStepfunctionsSendTaskSuccessOptions(
[property: CommandSwitch("--task-token")] string TaskToken,
[property: CommandSwitch("--task-output")] string TaskOutput
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}