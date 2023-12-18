using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "du", "device", "module", "show")]
public record AzIotDuDeviceModuleShowOptions(
[property: CommandSwitch("--account")] int Account,
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--instance")] string Instance,
[property: CommandSwitch("--module-id")] string ModuleId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}