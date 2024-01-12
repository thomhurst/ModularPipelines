using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("firebase", "test", "android", "list-device-capacities")]
public record GcloudFirebaseTestAndroidListDeviceCapacitiesOptions : GcloudOptions;