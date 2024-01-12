using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "api-configs", "delete")]
public record GcloudApiGatewayApiConfigsDeleteOptions(
[property: PositionalArgument] string ApiConfig,
[property: PositionalArgument] string Api
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }
}