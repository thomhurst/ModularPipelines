using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dt", "twin", "telemetry", "send")]
public record AzDtTwinTelemetrySendOptions(
[property: CommandSwitch("--dt-name")] string DtName,
[property: CommandSwitch("--twin-id")] string TwinId
) : AzOptions
{
    [CommandSwitch("--component")]
    public string? Component { get; set; }

    [CommandSwitch("--dt-id")]
    public string? DtId { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--telemetry")]
    public string? Telemetry { get; set; }

    [CommandSwitch("--telemetry-source-time")]
    public string? TelemetrySourceTime { get; set; }
}