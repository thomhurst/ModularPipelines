using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "image-version", "list")]
public record AzDevcenterAdminImageVersionListOptions(
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--image-name")] string ImageName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}