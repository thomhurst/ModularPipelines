using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "ops", "mq", "stats")]
public record AzIotOpsMqStatsOptions : AzOptions
{
    [CliOption("--context")]
    public string? Context { get; set; }

    [CliOption("--metrics-port")]
    public string? MetricsPort { get; set; }

    [CliOption("--namespace")]
    public string? Namespace { get; set; }

    [CliOption("--protobuf-port")]
    public string? ProtobufPort { get; set; }

    [CliFlag("--raw")]
    public bool? Raw { get; set; }

    [CliOption("--refresh")]
    public string? Refresh { get; set; }

    [CliOption("--trace-dir")]
    public string? TraceDir { get; set; }

    [CliOption("--trace-ids")]
    public string? TraceIds { get; set; }

    [CliFlag("--watch")]
    public bool? Watch { get; set; }
}