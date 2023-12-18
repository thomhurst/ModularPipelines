using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "registration-info")]
public record AzIotCentralDeviceRegistrationInfoOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}