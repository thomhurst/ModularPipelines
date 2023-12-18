using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "image", "list-installed")]
public record AzSphereDeviceImageListInstalledOptions : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [BooleanCommandSwitch("--full")]
    public bool? Full { get; set; }
}