using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "rollouts", "retry-job")]
public record GcloudDeployRolloutsRetryJobOptions(
[property: CliArgument] string Rollout,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region,
[property: CliArgument] string Release,
[property: CliOption("--job-id")] string JobId,
[property: CliOption("--phase-id")] string PhaseId
) : GcloudOptions;