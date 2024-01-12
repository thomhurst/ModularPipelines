using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "index-endpoints", "describe")]
public record GcloudAiIndexEndpointsDescribeOptions(
[property: PositionalArgument] string IndexEndpoint,
[property: PositionalArgument] string Region
) : GcloudOptions;