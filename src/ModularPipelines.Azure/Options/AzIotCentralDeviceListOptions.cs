using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "list")]
public record AzIotCentralDeviceListOptions(
[property: CommandSwitch("--app-id")] string AppId
) : AzOptions
{
    [CommandSwitch("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [BooleanCommandSwitch("--edge-only")]
    public bool? EdgeOnly { get; set; }

    [CommandSwitch("--token")]
    public string? Token { get; set; }
}