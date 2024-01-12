using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "apis", "describe")]
public record GcloudApiGatewayApisDescribeOptions(
[property: PositionalArgument] string Api
) : GcloudOptions;