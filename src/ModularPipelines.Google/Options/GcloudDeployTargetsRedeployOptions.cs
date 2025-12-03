using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "redeploy")]
public record GcloudDeployTargetsRedeployOptions(
[property: CliArgument] string Target,
[property: CliArgument] string Region,
[property: CliOption("--delivery-pipeline")] string DeliveryPipeline
) : GcloudOptions
{
    [CliOption("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CliOption("--rollout-id")]
    public string? RolloutId { get; set; }

    [CliOption("--starting-phase-id")]
    public string? StartingPhaseId { get; set; }
}