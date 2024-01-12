using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "index-endpoints", "delete")]
public record GcloudAiIndexEndpointsDeleteOptions(
[property: PositionalArgument] string IndexEndpoint,
[property: PositionalArgument] string Region
) : GcloudOptions;