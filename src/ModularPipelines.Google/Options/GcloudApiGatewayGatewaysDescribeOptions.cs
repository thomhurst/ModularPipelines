using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "gateways", "describe")]
public record GcloudApiGatewayGatewaysDescribeOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location
) : GcloudOptions;