using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "create-experiment")]
public record AwsEvidentlyCreateExperimentOptions(
[property: CliOption("--metric-goals")] string[] MetricGoals,
[property: CliOption("--name")] string Name,
[property: CliOption("--project")] string Project,
[property: CliOption("--treatments")] string[] Treatments
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--online-ab-config")]
    public string? OnlineAbConfig { get; set; }

    [CliOption("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CliOption("--sampling-rate")]
    public long? SamplingRate { get; set; }

    [CliOption("--segment")]
    public string? Segment { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}