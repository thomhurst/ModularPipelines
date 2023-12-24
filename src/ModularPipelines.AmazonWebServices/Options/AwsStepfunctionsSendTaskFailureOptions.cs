using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "send-task-failure")]
public record AwsStepfunctionsSendTaskFailureOptions(
[property: CommandSwitch("--task-token")] string TaskToken
) : AwsOptions
{
    [CommandSwitch("--error")]
    public string? Error { get; set; }

    [CommandSwitch("--cause")]
    public string? Cause { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}