using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "describe")]
public record GcloudAiEndpointsDescribeOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region
) : GcloudOptions;