using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "describe-state-machine-alias")]
public record AwsStepfunctionsDescribeStateMachineAliasOptions(
[property: CliOption("--state-machine-alias-arn")] string StateMachineAliasArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}