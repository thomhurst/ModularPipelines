using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "gateways", "create")]
public record GcloudApiGatewayGatewaysCreateOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location,
[property: CliOption("--api-config")] string ApiConfig,
[property: CliOption("--api")] string Api
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--labels")]
    public IEnumerable<KeyValue>? Labels { get; set; }
}