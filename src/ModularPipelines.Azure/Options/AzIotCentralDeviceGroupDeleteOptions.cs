using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device-group", "delete")]
public record AzIotCentralDeviceGroupDeleteOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-group-id")] string DeviceGroupId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}