using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "tls-routes", "describe")]
public record GcloudNetworkServicesTlsRoutesDescribeOptions(
[property: CliArgument] string TlsRoute,
[property: CliArgument] string Location
) : GcloudOptions;