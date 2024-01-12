using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "releases", "promote")]
public record GcloudDeployReleasesPromoteOptions(
[property: CommandSwitch("--release")] string Release,
[property: CommandSwitch("--delivery-pipeline")] string DeliveryPipeline,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--rollout-id")]
    public string? RolloutId { get; set; }

    [CommandSwitch("--starting-phase-id")]
    public string? StartingPhaseId { get; set; }

    [CommandSwitch("--to-target")]
    public string? ToTarget { get; set; }
}