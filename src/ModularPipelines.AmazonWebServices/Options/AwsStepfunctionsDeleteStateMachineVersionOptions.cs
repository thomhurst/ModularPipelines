using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "delete-state-machine-version")]
public record AwsStepfunctionsDeleteStateMachineVersionOptions(
[property: CommandSwitch("--state-machine-version-arn")] string StateMachineVersionArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}