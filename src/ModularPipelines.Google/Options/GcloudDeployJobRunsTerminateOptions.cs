using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "job-runs", "terminate")]
public record GcloudDeployJobRunsTerminateOptions(
[property: CliArgument] string JobRun,
[property: CliArgument] string DeliveryPipeline,
[property: CliArgument] string Region,
[property: CliArgument] string Release,
[property: CliArgument] string Rollout
) : GcloudOptions;