using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "job-runs", "describe")]
public record GcloudDeployJobRunsDescribeOptions(
[property: PositionalArgument] string JobRun,
[property: PositionalArgument] string DeliveryPipeline,
[property: PositionalArgument] string Region,
[property: PositionalArgument] string Release,
[property: PositionalArgument] string Rollout
) : GcloudOptions;