using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "create", "(image-gallery", "extension)")]
public record AzSigCreateImageGalleryExtensionOptions(
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--eula")]
    public string? Eula { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--permissions")]
    public string? Permissions { get; set; }

    [CliOption("--public-name-prefix")]
    public string? PublicNamePrefix { get; set; }

    [CliOption("--publisher-email")]
    public string? PublisherEmail { get; set; }

    [CliOption("--publisher-uri")]
    public string? PublisherUri { get; set; }

    [CliFlag("--soft-delete")]
    public bool? SoftDelete { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}