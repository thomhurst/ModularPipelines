using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "rollouts", "ignore-job")]
public record GcloudDeployRolloutsIgnoreJobOptions(
[property: PositionalArgument] string Rollout,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Release,
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--phase-id")] string PhaseId
) : GcloudOptions;