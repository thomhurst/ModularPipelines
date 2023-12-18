using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "create")]
public record AzIotCentralDeviceCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--device-name")]
    public string? DeviceName { get; set; }

    [CommandSwitch("--organizations")]
    public string? Organizations { get; set; }

    [BooleanCommandSwitch("--simulated")]
    public bool? Simulated { get; set; }

    [CommandSwitch("--template")]
    public string? Template { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}