using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("api-gateway", "gateways", "create")]
public record GcloudApiGatewayGatewaysCreateOptions(
[property: PositionalArgument] string Gateway,
[property: PositionalArgument] string Location,
[property: CommandSwitch("--api-config")] string ApiConfig,
[property: CommandSwitch("--api")] string Api
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}