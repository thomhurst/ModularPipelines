using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "create-launch")]
public record AwsEvidentlyCreateLaunchOptions(
[property: CliOption("--groups")] string[] Groups,
[property: CliOption("--name")] string Name,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--metric-monitors")]
    public string[]? MetricMonitors { get; set; }

    [CliOption("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CliOption("--scheduled-splits-config")]
    public string? ScheduledSplitsConfig { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}