using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sphere", "image", "add")]
public record AzSphereImageAddOptions(
[property: CommandSwitch("--catalog")] string Catalog,
[property: CommandSwitch("--image-path")] string ImagePath,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--regional-data-boundary")]
    public string? RegionalDataBoundary { get; set; }
}