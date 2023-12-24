using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "describe-state-machine-for-execution")]
public record AwsStepfunctionsDescribeStateMachineForExecutionOptions(
[property: CommandSwitch("--execution-arn")] string ExecutionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}