using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "connections", "describe")]
public record GcloudBuildsConnectionsDescribeOptions(
[property: CliArgument] string Connection,
[property: CliArgument] string Region
) : GcloudOptions;