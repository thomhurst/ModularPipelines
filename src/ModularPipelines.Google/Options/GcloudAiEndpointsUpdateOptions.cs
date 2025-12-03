using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ai", "endpoints", "update")]
public record GcloudAiEndpointsUpdateOptions(
[property: CliArgument] string Endpoint,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [CliFlag("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CliOption("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [CliFlag("--clear-traffic-split")]
    public bool? ClearTrafficSplit { get; set; }

    [CliOption("--traffic-split")]
    public string[]? TrafficSplit { get; set; }

    [CliFlag("--disable-request-response-logging")]
    public bool? DisableRequestResponseLogging { get; set; }

    [CliOption("--request-response-logging-rate")]
    public string? RequestResponseLoggingRate { get; set; }

    [CliOption("--request-response-logging-table")]
    public string? RequestResponseLoggingTable { get; set; }
}