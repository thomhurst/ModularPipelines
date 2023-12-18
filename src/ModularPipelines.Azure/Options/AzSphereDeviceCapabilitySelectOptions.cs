using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "select")]
public record AzSphereDeviceCapabilitySelectOptions : AzOptions
{
    [CommandSwitch("--capability-file")]
    public string? CapabilityFile { get; set; }

    [BooleanCommandSwitch("--none")]
    public bool? None { get; set; }
}