using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("firebase", "test", "ios", "list-device-capacities")]
public record GcloudFirebaseTestIosListDeviceCapacitiesOptions : GcloudOptions;