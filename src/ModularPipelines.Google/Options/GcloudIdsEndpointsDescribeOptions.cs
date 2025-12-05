using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ids", "endpoints", "describe")]
public record GcloudIdsEndpointsDescribeOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Zone
) : GcloudOptions;