using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "index-endpoints", "undeploy-index")]
public record GcloudAiIndexEndpointsUndeployIndexOptions(
[property: PositionalArgument] string IndexEndpoint,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--deployed-index-id")] string DeployedIndexId
) : GcloudOptions;