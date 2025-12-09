using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device", "update")]
public record AzIotCentralDeviceUpdateOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-id")] string DeviceId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--device-name")]
    public string? DeviceName { get; set; }

    [CliFlag("--enable")]
    public bool? Enable { get; set; }

    [CliOption("--organizations")]
    public string? Organizations { get; set; }

    [CliFlag("--simulated")]
    public bool? Simulated { get; set; }

    [CliOption("--template")]
    public string? Template { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}