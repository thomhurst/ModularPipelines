using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "index-endpoints", "undeploy-index")]
public record GcloudAiIndexEndpointsUndeployIndexOptions(
[property: CliArgument] string IndexEndpoint,
[property: CliArgument] string Region,
[property: CliOption("--deployed-index-id")] string DeployedIndexId
) : GcloudOptions;