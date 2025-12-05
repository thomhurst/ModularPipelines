using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("stepfunctions", "publish-state-machine-version")]
public record AwsStepfunctionsPublishStateMachineVersionOptions(
[property: CliOption("--state-machine-arn")] string StateMachineArn
) : AwsOptions
{
    [CliOption("--revision-id")]
    public string? RevisionId { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}