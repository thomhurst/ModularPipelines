using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "telemetry", "show")]
public record AzIotCentralDeviceTelemetryShowOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--telemetry-name--tn")] string TelemetryNameTn
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--co")]
    public string? Co { get; set; }

    [CommandSwitch("--mn")]
    public string? Mn { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}