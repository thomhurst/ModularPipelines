using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amlfs", "check-amlfs-subnet")]
public record AzAmlfsCheckAmlfsSubnetOptions(
[property: CommandSwitch("--aml-filesystem-name")] string AmlFilesystemName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
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

