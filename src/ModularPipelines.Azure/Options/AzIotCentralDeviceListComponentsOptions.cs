using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "list-components")]
public record AzIotCentralDeviceListComponentsOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--mn")]
    public string? Mn { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}