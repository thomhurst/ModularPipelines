using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "rollouts", "approve")]
public record GcloudDeployRolloutsApproveOptions(
[property: PositionalArgument] string Rollout,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Release
) : GcloudOptions;