using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "get-experiment-results")]
public record AwsEvidentlyGetExperimentResultsOptions(
[property: CliOption("--experiment")] string Experiment,
[property: CliOption("--metric-names")] string[] MetricNames,
[property: CliOption("--project")] string Project,
[property: CliOption("--treatment-names")] string[] TreatmentNames
) : AwsOptions
{
    [CliOption("--base-stat")]
    public string? BaseStat { get; set; }

    [CliOption("--end-time")]
    public long? EndTime { get; set; }

    [CliOption("--period")]
    public long? Period { get; set; }

    [CliOption("--report-names")]
    public string[]? ReportNames { get; set; }

    [CliOption("--result-stats")]
    public string[]? ResultStats { get; set; }

    [CliOption("--start-time")]
    public long? StartTime { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}