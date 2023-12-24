using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "wait", "transform-job-completed-or-stopped")]
public record AwsSagemakerWaitTransformJobCompletedOrStoppedOptions(
[property: CommandSwitch("--transform-job-name")] string TransformJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}