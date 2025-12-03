using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "capability", "select")]
public record AzSphereDeviceCapabilitySelectOptions : AzOptions
{
    [CliOption("--capability-file")]
    public string? CapabilityFile { get; set; }

    [CliFlag("--none")]
    public bool? None { get; set; }
}