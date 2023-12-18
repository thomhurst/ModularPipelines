using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device-template", "show")]
public record AzIotCentralDeviceTemplateShowOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-template-id")] string DeviceTemplateId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}