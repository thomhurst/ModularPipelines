using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "apis", "describe")]
public record GcloudApiGatewayApisDescribeOptions(
[property: CliArgument] string Api
) : GcloudOptions;