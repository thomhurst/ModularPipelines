using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "tcp-routes", "describe")]
public record GcloudNetworkServicesTcpRoutesDescribeOptions(
[property: CliArgument] string TcpRoute,
[property: CliArgument] string Location
) : GcloudOptions;