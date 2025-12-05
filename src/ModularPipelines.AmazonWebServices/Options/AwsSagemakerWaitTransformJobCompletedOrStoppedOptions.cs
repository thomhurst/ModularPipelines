using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "wait", "transform-job-completed-or-stopped")]
public record AwsSagemakerWaitTransformJobCompletedOrStoppedOptions(
[property: CliOption("--transform-job-name")] string TransformJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}