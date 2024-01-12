using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ai", "endpoints", "update")]
public record GcloudAiEndpointsUpdateOptions(
[property: PositionalArgument] string Endpoint,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--update-labels")]
    public IEnumerable<KeyValue>? UpdateLabels { get; set; }

    [BooleanCommandSwitch("--clear-labels")]
    public bool? ClearLabels { get; set; }

    [CommandSwitch("--remove-labels")]
    public string[]? RemoveLabels { get; set; }

    [BooleanCommandSwitch("--clear-traffic-split")]
    public bool? ClearTrafficSplit { get; set; }

    [CommandSwitch("--traffic-split")]
    public string[]? TrafficSplit { get; set; }

    [BooleanCommandSwitch("--disable-request-response-logging")]
    public bool? DisableRequestResponseLogging { get; set; }

    [CommandSwitch("--request-response-logging-rate")]
    public string? RequestResponseLoggingRate { get; set; }

    [CommandSwitch("--request-response-logging-table")]
    public string? RequestResponseLoggingTable { get; set; }
}