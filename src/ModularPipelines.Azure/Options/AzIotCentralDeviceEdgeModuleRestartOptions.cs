using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device", "edge", "module", "restart")]
public record AzIotCentralDeviceEdgeModuleRestartOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--module-id")] string ModuleId
) : AzOptions
{
    [CliOption("--central-api-uri")]
    public string? CentralApiUri { get; set; }

    [CliOption("--token")]
    public string? Token { get; set; }
}