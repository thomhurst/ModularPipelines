using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sagemaker", "stop-hyper-parameter-tuning-job")]
public record AwsSagemakerStopHyperParameterTuningJobOptions(
[property: CommandSwitch("--hyper-parameter-tuning-job-name")] string HyperParameterTuningJobName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}