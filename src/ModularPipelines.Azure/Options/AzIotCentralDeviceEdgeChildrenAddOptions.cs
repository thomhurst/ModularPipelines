using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device", "edge", "children", "add")]
public record AzIotCentralDeviceEdgeChildrenAddOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--children-ids")] string ChildrenIds,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}