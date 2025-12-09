using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "send-workflow-step-action")]
public record AwsImagebuilderSendWorkflowStepActionOptions(
[property: CliOption("--step-execution-id")] string StepExecutionId,
[property: CliOption("--image-build-version-arn")] string ImageBuildVersionArn,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}