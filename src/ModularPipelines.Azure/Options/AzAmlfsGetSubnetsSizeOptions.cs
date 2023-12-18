using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("amlfs", "get-subnets-size")]
public record AzAmlfsGetSubnetsSizeOptions : AzOptions
{
    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--storage-capacity")]
    public string? StorageCapacity { get; set; }
}

