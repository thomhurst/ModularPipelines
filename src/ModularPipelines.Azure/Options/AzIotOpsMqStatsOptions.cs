using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "ops", "mq", "stats")]
public record AzIotOpsMqStatsOptions : AzOptions
{
    [CommandSwitch("--context")]
    public string? Context { get; set; }

    [CommandSwitch("--metrics-port")]
    public string? MetricsPort { get; set; }

    [CommandSwitch("--namespace")]
    public string? Namespace { get; set; }

    [CommandSwitch("--protobuf-port")]
    public string? ProtobufPort { get; set; }

    [BooleanCommandSwitch("--raw")]
    public bool? Raw { get; set; }

    [CommandSwitch("--refresh")]
    public string? Refresh { get; set; }

    [CommandSwitch("--trace-dir")]
    public string? TraceDir { get; set; }

    [CommandSwitch("--trace-ids")]
    public string? TraceIds { get; set; }

    [BooleanCommandSwitch("--watch")]
    public bool? Watch { get; set; }
}