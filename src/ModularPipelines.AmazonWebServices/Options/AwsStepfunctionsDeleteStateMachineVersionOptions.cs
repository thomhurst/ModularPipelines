using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "delete-state-machine-version")]
public record AwsStepfunctionsDeleteStateMachineVersionOptions(
[property: CliOption("--state-machine-version-arn")] string StateMachineVersionArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}