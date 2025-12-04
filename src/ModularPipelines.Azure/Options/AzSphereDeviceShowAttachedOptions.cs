using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "show-attached")]
public record AzSphereDeviceShowAttachedOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}