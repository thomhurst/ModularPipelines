using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("devcenter", "admin", "image", "show")]
public record AzDevcenterAdminImageShowOptions : AzOptions
{
    [CliOption("--dev-center")]
    public string? DevCenter { get; set; }

    [CliOption("--gallery-name")]
    public string? GalleryName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--image-name")]
    public string? ImageName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}