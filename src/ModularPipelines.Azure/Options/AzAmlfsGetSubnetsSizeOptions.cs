using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("amlfs", "get-subnets-size")]
public record AzAmlfsGetSubnetsSizeOptions : AzOptions
{
    [CliOption("--sku")]
    public string? Sku { get; set; }

    [CliOption("--storage-capacity")]
    public string? StorageCapacity { get; set; }
}