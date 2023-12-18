using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device-group", "create")]
public record AzIotCentralDeviceGroupCreateOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--device-group-id")] string DeviceGroupId,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--filter")] string Filter
) : AzOptions
{
    [CommandSwitch("--api-version")]
    public string? ApiVersion { get; set; }

    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--organizations")]
    public string? Organizations { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}