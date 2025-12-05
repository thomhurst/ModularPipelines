using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device-group", "show")]
public record AzIotCentralDeviceGroupShowOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-group-id")] string DeviceGroupId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}