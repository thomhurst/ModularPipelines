using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "wait", "training-job-completed-or-stopped")]
public record AwsSagemakerWaitTrainingJobCompletedOrStoppedOptions(
[property: CliOption("--training-job-name")] string TrainingJobName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}