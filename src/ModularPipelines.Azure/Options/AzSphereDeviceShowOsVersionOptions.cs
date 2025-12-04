using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "show-os-version")]
public record AzSphereDeviceShowOsVersionOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}