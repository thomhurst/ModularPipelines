using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "capability", "show-attached")]
public record AzSphereDeviceCapabilityShowAttachedOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}