using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amlfs", "check-amlfs-subnet")]
public record AzAmlfsCheckAmlfsSubnetOptions : AzOptions
{
    [CliOption("--filesystem-subnet")]
    public string? FilesystemSubnet { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storage-capacity")]
    public string? StorageCapacity { get; set; }
}