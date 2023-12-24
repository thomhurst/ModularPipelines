using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stepfunctions", "publish-state-machine-version")]
public record AwsStepfunctionsPublishStateMachineVersionOptions(
[property: CommandSwitch("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CommandSwitch("--revision-id")]
    public string? RevisionId { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}