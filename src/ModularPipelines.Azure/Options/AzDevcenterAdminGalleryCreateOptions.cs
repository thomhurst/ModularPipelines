using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devcenter", "admin", "gallery", "create")]
public record AzDevcenterAdminGalleryCreateOptions(
[property: CommandSwitch("--dev-center")] string DevCenter,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--gallery-resource-id")] string GalleryResourceId,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }
}

