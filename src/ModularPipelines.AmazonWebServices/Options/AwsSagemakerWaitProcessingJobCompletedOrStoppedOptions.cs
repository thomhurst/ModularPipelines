using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "wait", "processing-job-completed-or-stopped")]
public record AwsSagemakerWaitProcessingJobCompletedOrStoppedOptions(
[property: CommandSwitch("--processing-job-name")] string ProcessingJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}