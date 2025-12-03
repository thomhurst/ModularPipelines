using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devcenter", "admin", "image", "list")]
public record AzDevcenterAdminImageListOptions(
[property: CliOption("--dev-center")] string DevCenter,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--gallery-name")]
    public string? GalleryName { get; set; }

    [CliOption("--max-items")]
    public string? MaxItems { get; set; }

    [CliOption("--next-token")]
    public string? NextToken { get; set; }
}