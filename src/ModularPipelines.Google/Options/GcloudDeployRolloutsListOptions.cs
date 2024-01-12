using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "rollouts", "list")]
public record GcloudDeployRolloutsListOptions(
[property: CommandSwitch("--release")] string Release,
[property: CommandSwitch("--delivery-pipeline")] string DeliveryPipeline,
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;