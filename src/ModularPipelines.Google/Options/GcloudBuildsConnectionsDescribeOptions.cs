using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("builds", "connections", "describe")]
public record GcloudBuildsConnectionsDescribeOptions(
[property: PositionalArgument] string Connection,
[property: PositionalArgument] string Region
) : GcloudOptions;