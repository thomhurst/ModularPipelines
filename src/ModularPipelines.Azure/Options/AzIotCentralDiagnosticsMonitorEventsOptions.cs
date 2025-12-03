using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "diagnostics", "monitor-events")]
public record AzIotCentralDiagnosticsMonitorEventsOptions(
[property: CliOption("--app-id")] string AppId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--cg")]
    public string? Cg { get; set; }

    [CliOption("--device-id")]
    public string? DeviceId { get; set; }

    [CliOption("--enqueued-time")]
    public string? EnqueuedTime { get; set; }

    [CliOption("--module-id")]
    public string? ModuleId { get; set; }

    [CliOption("--properties")]
    public string? Properties { get; set; }

    [CliFlag("--repair")]
    public bool? Repair { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}