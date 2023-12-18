using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iot", "central", "device", "compute-device-key")]
public record AzIotCentralDeviceComputeDeviceKeyOptions(
[property: CommandSwitch("--device-id")] string DeviceId,
[property: CommandSwitch("--pk")] string Pk
) : AzOptions
{
}