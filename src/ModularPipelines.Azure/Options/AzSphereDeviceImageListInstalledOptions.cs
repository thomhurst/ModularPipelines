using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sphere", "device", "image", "list-installed")]
public record AzSphereDeviceImageListInstalledOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliFlag("--full")]
    public bool? Full { get; set; }
}