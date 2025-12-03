using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network-services", "gateways", "describe")]
public record GcloudNetworkServicesGatewaysDescribeOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location
) : GcloudOptions;