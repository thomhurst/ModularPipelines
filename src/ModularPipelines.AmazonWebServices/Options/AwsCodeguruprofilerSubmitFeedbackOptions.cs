using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("codeguruprofiler", "submit-feedback")]
public record AwsCodeguruprofilerSubmitFeedbackOptions(
[property: CommandSwitch("--anomaly-instance-id")] string AnomalyInstanceId,
[property: CommandSwitch("--profiling-group-name")] string ProfilingGroupName,
[property: CommandSwitch("--type")] string Type
) : AwsOptions
{
    [CommandSwitch("--comment")]
    public string? Comment { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}