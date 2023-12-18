using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device-group", "update")]
public record AzIotCentralDeviceGroupUpdateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-group-id")] string DeviceGroupId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }

    [CommandSwitch("--organizations")]
    public string? Organizations { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}