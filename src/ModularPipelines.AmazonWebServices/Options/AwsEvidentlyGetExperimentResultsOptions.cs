using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "get-experiment-results")]
public record AwsEvidentlyGetExperimentResultsOptions(
[property: CommandSwitch("--experiment")] string Experiment,
[property: CommandSwitch("--metric-names")] string[] MetricNames,
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--treatment-names")] string[] TreatmentNames
) : AwsOptions
{
    [CommandSwitch("--base-stat")]
    public string? BaseStat { get; set; }

    [CommandSwitch("--end-time")]
    public long? EndTime { get; set; }

    [CommandSwitch("--period")]
    public long? Period { get; set; }

    [CommandSwitch("--report-names")]
    public string[]? ReportNames { get; set; }

    [CommandSwitch("--result-stats")]
    public string[]? ResultStats { get; set; }

    [CommandSwitch("--start-time")]
    public long? StartTime { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}