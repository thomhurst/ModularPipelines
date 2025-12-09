using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device-group", "create")]
public record AzIotCentralDeviceGroupCreateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-group-id")] string DeviceGroupId,
[property: CliOption("--display-name")] string DisplayName,
[property: CliOption("--filter")] string Filter
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--organizations")]
    public string? Organizations { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}