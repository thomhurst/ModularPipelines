using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "rollouts", "list")]
public record GcloudDeployRolloutsListOptions(
[property: CliOption("--release")] string Release,
[property: CliOption("--delivery-pipeline")] string DeliveryPipeline,
[property: CliOption("--region")] string Region
) : GcloudOptions;