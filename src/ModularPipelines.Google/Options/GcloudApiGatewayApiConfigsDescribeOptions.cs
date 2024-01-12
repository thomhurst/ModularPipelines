using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "api-configs", "describe")]
public record GcloudApiGatewayApiConfigsDescribeOptions(
[property: PositionalArgument] string ApiConfig,
[property: PositionalArgument] string Api
) : GcloudOptions
{
    [CommandSwitch("--view")]
    public string? View { get; set; }
}