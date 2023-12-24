using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "wait", "training-job-completed-or-stopped")]
public record AwsSagemakerWaitTrainingJobCompletedOrStoppedOptions(
[property: CommandSwitch("--training-job-name")] string TrainingJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}