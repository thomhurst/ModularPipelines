using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "sideload", "deploy")]
public record AzSphereDeviceSideloadDeployOptions(
[property: CliOption("--image-package")] string ImagePackage
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliFlag("--manual-start")]
    public bool? ManualStart { get; set; }
}