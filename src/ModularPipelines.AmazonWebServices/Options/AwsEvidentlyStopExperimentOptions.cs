using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "stop-experiment")]
public record AwsEvidentlyStopExperimentOptions(
[property: CliOption("--experiment")] string Experiment,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--desired-state")]
    public string? DesiredState { get; set; }

    [CliOption("--reason")]
    public string? Reason { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}