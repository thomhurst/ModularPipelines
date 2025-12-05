using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("stack-hci-vm", "image", "create")]
public record AzStackHciVmImageCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--os-type")] string OsType,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--image-path")]
    public string? ImagePath { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--offer")]
    public string? Offer { get; set; }

    [CliOption("--polling-interval")]
    public string? PollingInterval { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storage-path-id")]
    public string? StoragePathId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--urn")]
    public string? Urn { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}