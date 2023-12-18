using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "capability", "select")]
public record AzSphereDeviceCapabilitySelectOptions(
[property: CommandSwitch("--capability-file")] string CapabilityFile
) : AzOptions
{
    [BooleanCommandSwitch("--none")]
    public bool? None { get; set; }
}