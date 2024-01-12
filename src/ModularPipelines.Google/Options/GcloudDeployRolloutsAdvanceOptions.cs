using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "rollouts", "advance")]
public record GcloudDeployRolloutsAdvanceOptions(
[property: PositionalArgument] string Rollout,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Release
) : GcloudOptions
{
    [CommandSwitch("--phase-id")]
    public string? PhaseId { get; set; }
}