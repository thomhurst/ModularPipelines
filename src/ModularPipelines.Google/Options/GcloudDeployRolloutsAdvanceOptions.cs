using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "rollouts", "advance")]
public record GcloudDeployRolloutsAdvanceOptions(
[property: CliArgument] string Rollout,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region,
[property: CliArgument] string Release
) : GcloudOptions
{
    [CliOption("--phase-id")]
    public string? PhaseId { get; set; }
}