using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network-services", "gateways", "describe")]
public record GcloudNetworkServicesGatewaysDescribeOptions(
[property: PositionalArgument] string Gateway,
[property: PositionalArgument] string Location
) : GcloudOptions;