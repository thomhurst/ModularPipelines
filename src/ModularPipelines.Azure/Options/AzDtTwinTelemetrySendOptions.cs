using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dt", "twin", "telemetry", "send")]
public record AzDtTwinTelemetrySendOptions(
[property: CliOption("--dt-name")] string DtName,
[property: CliOption("--twin-id")] string TwinId
) : AzOptions
{
    [CliOption("--component")]
    public string? Component { get; set; }

    [CliOption("--dt-id")]
    public string? DtId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--telemetry")]
    public string? Telemetry { get; set; }

    [CliOption("--telemetry-source-time")]
    public string? TelemetrySourceTime { get; set; }
}