using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "gallery", "create")]
public record AzDevcenterAdminGalleryCreateOptions(
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--gallery-resource-id")] string GalleryResourceId,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}