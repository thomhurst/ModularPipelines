using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cache", "services", "describe")]
public record GcloudEdgeCacheServicesDescribeOptions(
[property: CliArgument] string Service,
[property: CliArgument] string Location
) : GcloudOptions;