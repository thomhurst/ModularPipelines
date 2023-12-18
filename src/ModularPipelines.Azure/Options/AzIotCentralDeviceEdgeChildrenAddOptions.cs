using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "edge", "children", "add")]
public record AzIotCentralDeviceEdgeChildrenAddOptions(
[property: CommandSwitch("--app-id")] string AppId,
[property: CommandSwitch("--children-ids")] string ChildrenIds,
[property: CommandSwitch("--device-id")] string DeviceId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}