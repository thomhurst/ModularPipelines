using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "show-os-version")]
public record AzSphereDeviceShowOsVersionOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }
}