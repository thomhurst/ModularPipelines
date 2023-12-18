using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "update")]
public record AzIotCentralDeviceUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [BooleanCommandSwitch("--enable")]
    public bool? Enable { get; set; }

    [CommandSwitch("--organizations")]
    public string? Organizations { get; set; }

    [BooleanCommandSwitch("--simulated")]
    public bool? Simulated { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}

