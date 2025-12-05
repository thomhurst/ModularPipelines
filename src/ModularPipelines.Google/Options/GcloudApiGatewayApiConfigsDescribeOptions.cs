using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "api-configs", "describe")]
public record GcloudApiGatewayApiConfigsDescribeOptions(
[property: CliArgument] string ApiConfig,
[property: CliArgument] string Api
) : GcloudOptions
{
    [CliOption("--view")]
    public string? View { get; set; }
}