using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "device", "sideload", "deploy")]
public record AzSphereDeviceSideloadDeployOptions(
[property: CommandSwitch("--image-package")] string ImagePackage
) : AzOptions
{
    [CommandSwitch("--device")]
    public string? Device { get; set; }

    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }

    [BooleanCommandSwitch("--manual-start")]
    public bool? ManualStart { get; set; }
}