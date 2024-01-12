using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "tls-routes", "describe")]
public record GcloudNetworkServicesTlsRoutesDescribeOptions(
[property: PositionalArgument] string TlsRoute,
[property: PositionalArgument] string Location
) : GcloudOptions;