using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "http-routes", "describe")]
public record GcloudNetworkServicesHttpRoutesDescribeOptions(
[property: PositionalArgument] string HttpRoute,
[property: PositionalArgument] string Location
) : GcloudOptions;