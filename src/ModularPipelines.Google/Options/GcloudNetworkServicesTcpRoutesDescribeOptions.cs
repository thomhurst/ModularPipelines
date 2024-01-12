using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "tcp-routes", "describe")]
public record GcloudNetworkServicesTcpRoutesDescribeOptions(
[property: PositionalArgument] string TcpRoute,
[property: PositionalArgument] string Location
) : GcloudOptions;