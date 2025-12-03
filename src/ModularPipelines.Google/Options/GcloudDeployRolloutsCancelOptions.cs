using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "rollouts", "cancel")]
public record GcloudDeployRolloutsCancelOptions(
[property: CliArgument] string Rollout,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region,
[property: CliArgument] string Release
) : GcloudOptions;