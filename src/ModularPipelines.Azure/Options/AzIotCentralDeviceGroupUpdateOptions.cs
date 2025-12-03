using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("iot", "central", "device-group", "update")]
public record AzIotCentralDeviceGroupUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-group-id")] string DeviceGroupId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--organizations")]
    public string? Organizations { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}