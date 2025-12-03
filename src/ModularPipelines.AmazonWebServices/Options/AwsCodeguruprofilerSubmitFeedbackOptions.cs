using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("codeguruprofiler", "submit-feedback")]
public record AwsCodeguruprofilerSubmitFeedbackOptions(
[property: CliOption("--anomaly-instance-id")] string AnomalyInstanceId,
[property: CliOption("--profiling-group-name")] string ProfilingGroupName,
[property: CliOption("--type")] string Type
) : AwsOptions
{
    [CliOption("--comment")]
    public string? Comment { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}