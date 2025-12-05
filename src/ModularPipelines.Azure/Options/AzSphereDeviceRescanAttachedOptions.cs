using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "rescan-attached")]
public record AzSphereDeviceRescanAttachedOptions : AzOptions;