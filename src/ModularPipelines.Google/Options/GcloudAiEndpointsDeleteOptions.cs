using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "delete")]
public record GcloudAiEndpointsDeleteOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region
) : GcloudOptions;