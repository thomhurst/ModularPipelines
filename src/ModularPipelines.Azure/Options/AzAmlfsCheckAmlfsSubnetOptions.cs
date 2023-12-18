using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amlfs", "check-amlfs-subnet")]
public record AzAmlfsCheckAmlfsSubnetOptions : AzOptions
{
    [CommandSwitch("--filesystem-subnet")]
    public string? FilesystemSubnet { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--storage-capacity")]
    public string? StorageCapacity { get; set; }
}