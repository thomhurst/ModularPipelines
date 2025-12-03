using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "start-experiment")]
public record AwsEvidentlyStartExperimentOptions(
[property: CliOption("--analysis-complete-time")] long AnalysisCompleteTime,
[property: CliOption("--experiment")] string Experiment,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}