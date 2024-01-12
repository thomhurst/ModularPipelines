using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ids", "endpoints", "describe")]
public record GcloudIdsEndpointsDescribeOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Zone
) : GcloudOptions;