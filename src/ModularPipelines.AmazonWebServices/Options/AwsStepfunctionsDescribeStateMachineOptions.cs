using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "describe-state-machine")]
public record AwsStepfunctionsDescribeStateMachineOptions(
[property: CliOption("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}