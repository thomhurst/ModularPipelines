using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "apis", "delete")]
public record GcloudApiGatewayApisDeleteOptions(
[property: CliArgument] string Api
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }
}