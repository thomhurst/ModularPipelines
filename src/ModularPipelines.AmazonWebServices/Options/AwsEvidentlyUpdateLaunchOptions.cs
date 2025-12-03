using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("evidently", "update-launch")]
public record AwsEvidentlyUpdateLaunchOptions(
[property: CliOption("--launch")] string Launch,
[property: CliOption("--project")] string Project
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--groups")]
    public string[]? Groups { get; set; }

    [CliOption("--metric-monitors")]
    public string[]? MetricMonitors { get; set; }

    [CliOption("--randomization-salt")]
    public string? RandomizationSalt { get; set; }

    [CliOption("--scheduled-splits-config")]
    public string? ScheduledSplitsConfig { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}