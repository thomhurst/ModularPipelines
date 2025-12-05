using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "job-runs", "list")]
public record GcloudDeployJobRunsListOptions(
[property: CliOption("--rollout")] string Rollout,
[property: CliOption("--delivery-pipeline")] string DeliveryPipeline,
[property: CliOption("--region")] string Region,
[property: CliOption("--release")] string Release
) : GcloudOptions;