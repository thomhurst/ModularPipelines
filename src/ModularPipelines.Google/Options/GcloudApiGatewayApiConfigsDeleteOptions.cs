using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "api-configs", "delete")]
public record GcloudApiGatewayApiConfigsDeleteOptions(
[property: CliArgument] string ApiConfig,
[property: CliArgument] string Api
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}