using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "device-group", "delete")]
public record AzIotCentralDeviceGroupDeleteOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-group-id")] string DeviceGroupId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}