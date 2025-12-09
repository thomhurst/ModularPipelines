using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("iot", "central", "device", "compute-device-key")]
public record AzIotCentralDeviceComputeDeviceKeyOptions(
[property: CliOption("--device-id")] string DeviceId,
[property: CliOption("--pk")] string Pk
) : AzOptions;