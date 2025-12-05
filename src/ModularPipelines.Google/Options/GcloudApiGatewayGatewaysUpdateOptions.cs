using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("api-gateway", "gateways", "update")]
public record GcloudApiGatewayGatewaysUpdateOptions(
[property: CliArgument] string Gateway,
[property: CliArgument] string Location
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliOption("--api-config")]
    public string? ApiConfig { get; set; }

    [CliOption("--api")]
    public string? Api { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }
}