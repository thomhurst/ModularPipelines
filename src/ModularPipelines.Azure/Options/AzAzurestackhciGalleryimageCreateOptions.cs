using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("urestackhci", "galleryimage", "create")]
public record AzAzurestackhciGalleryimageCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CliOption("--image-path")]
    public string? ImagePath { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--offer")]
    public string? Offer { get; set; }

    [CliOption("--os-type")]
    public string? OsType { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storagepath-id")]
    public string? StoragepathId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}