using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "describe")]
public record GcloudAiEndpointsDescribeOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region
) : GcloudOptions;