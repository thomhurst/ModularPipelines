using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "http-routes", "describe")]
public record GcloudNetworkServicesHttpRoutesDescribeOptions(
[property: CliArgument] string HttpRoute,
[property: CliArgument] string Location
) : GcloudOptions;