using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "targets", "redeploy")]
public record GcloudDeployTargetsRedeployOptions(
[property: PositionalArgument] string Target,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--delivery-pipeline")] string DeliveryPipeline
) : GcloudOptions
{
    [CommandSwitch("--annotations")]
    public IEnumerable<KeyValue>? Annotations { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }

    [CommandSwitch("--rollout-id")]
    public string? RolloutId { get; set; }

    [CommandSwitch("--starting-phase-id")]
    public string? StartingPhaseId { get; set; }
}