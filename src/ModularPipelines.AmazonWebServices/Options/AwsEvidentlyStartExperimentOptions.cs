using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "start-experiment")]
public record AwsEvidentlyStartExperimentOptions(
[property: CommandSwitch("--analysis-complete-time")] long AnalysisCompleteTime,
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}