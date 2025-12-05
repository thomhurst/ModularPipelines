using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "index-endpoints", "describe")]
public record GcloudAiIndexEndpointsDescribeOptions(
[property: CliArgument] string IndexEndpoint,
[property: CliArgument] string Region
) : GcloudOptions;