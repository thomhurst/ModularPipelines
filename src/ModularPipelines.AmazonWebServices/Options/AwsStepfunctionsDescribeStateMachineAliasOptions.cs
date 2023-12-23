using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "describe-state-machine-alias")]
public record AwsStepfunctionsDescribeStateMachineAliasOptions(
[property: CommandSwitch("--state-machine-alias-arn")] string StateMachineAliasArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}