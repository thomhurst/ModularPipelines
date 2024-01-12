using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "rollouts", "retry-job")]
public record GcloudDeployRolloutsRetryJobOptions(
[property: PositionalArgument] string Rollout,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Release,
[property: CommandSwitch("--job-id")] string JobId,
[property: CommandSwitch("--phase-id")] string PhaseId
) : GcloudOptions;