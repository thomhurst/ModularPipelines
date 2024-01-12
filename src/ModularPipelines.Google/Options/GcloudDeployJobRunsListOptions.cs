using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "job-runs", "list")]
public record GcloudDeployJobRunsListOptions(
[property: CommandSwitch("--rollout")] string Rollout,
[property: CommandSwitch("--delivery-pipeline")] string DeliveryPipeline,
[property: CommandSwitch("--region")] string Region,
[property: CommandSwitch("--release")] string Release
) : GcloudOptions;