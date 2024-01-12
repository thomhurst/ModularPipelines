using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "gateways", "describe")]
public record GcloudApiGatewayGatewaysDescribeOptions(
[property: PositionalArgument] string Gateway,
[property: PositionalArgument] string Location
) : GcloudOptions;