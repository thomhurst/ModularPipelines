using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("urestackhci", "galleryimage", "create")]
public record AzAzurestackhciGalleryimageCreateOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--extended-location")]
    public string? ExtendedLocation { get; set; }

    [CommandSwitch("--image-path")]
    public string? ImagePath { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--offer")]
    public string? Offer { get; set; }

    [CommandSwitch("--os-type")]
    public string? OsType { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--storagepath-id")]
    public string? StoragepathId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}