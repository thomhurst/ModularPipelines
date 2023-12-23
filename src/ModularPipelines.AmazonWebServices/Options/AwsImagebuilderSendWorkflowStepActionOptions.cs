using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("imagebuilder", "send-workflow-step-action")]
public record AwsImagebuilderSendWorkflowStepActionOptions(
[property: CommandSwitch("--step-execution-id")] string StepExecutionId,
[property: CommandSwitch("--image-build-version-arn")] string ImageBuildVersionArn,
[property: CommandSwitch("--action")] string Action
) : AwsOptions
{
    [CommandSwitch("--reason")]
    public string? Reason { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}