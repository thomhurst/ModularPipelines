using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "image", "list-targeted")]
public record AzSphereImageListTargetedOptions(
[property: CliOption("--catalog")] string Catalog,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }

    [CliFlag("--full")]
    public bool? Full { get; set; }
}