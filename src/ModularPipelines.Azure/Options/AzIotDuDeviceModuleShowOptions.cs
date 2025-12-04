using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "du", "device", "module", "show")]
public record AzIotDuDeviceModuleShowOptions(
[property: CliOption("--account")] int Account,
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--instance")] string Instance,
[property: CliOption("--module-id")] string ModuleId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}