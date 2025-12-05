using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "releases", "promote")]
public record GcloudDeployReleasesPromoteOptions(
[property: CliOption("--release")] string Release,
[property: CliOption("--delivery-pipeline")] string DeliveryPipeline,
[property: CliOption("--region")] string Region
) : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--rollout-id")]
    public string? RolloutId { get; set; }

    [CliOption("--starting-phase-id")]
    public string? StartingPhaseId { get; set; }

    [CliOption("--to-target")]
    public string? ToTarget { get; set; }
}