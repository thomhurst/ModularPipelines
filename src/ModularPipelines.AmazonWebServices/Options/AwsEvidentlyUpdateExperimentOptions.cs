using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "update-experiment")]
public record AwsEvidentlyUpdateExperimentOptions(
[property: CliOption("--experiment")] string Experiment,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--metric-goals")]
    public string[]? MetricGoals { get; set; }

    [CliOption("--online-ab-config")]
    public string? OnlineAbConfig { get; set; }

    [CliOption("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CliOption("--sampling-rate")]
    public long? SamplingRate { get; set; }

    [CliOption("--segment")]
    public string? Segment { get; set; }

    [CliOption("--treatments")]
    public string[]? Treatments { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}