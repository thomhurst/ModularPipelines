using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "describe-state-machine-for-execution")]
public record AwsStepfunctionsDescribeStateMachineForExecutionOptions(
[property: CliOption("--execution-arn")] string ExecutionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}