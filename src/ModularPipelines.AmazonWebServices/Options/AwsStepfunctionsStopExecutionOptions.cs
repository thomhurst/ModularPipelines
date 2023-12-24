using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "stop-execution")]
public record AwsStepfunctionsStopExecutionOptions(
[property: CommandSwitch("--execution-arn")] string ExecutionArn
) : AwsOptions
{
    [CommandSwitch("--error")]
    public string? Error { get; set; }

    [CommandSwitch("--cause")]
    public string? Cause { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}