using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "image", "list-offers")]
public record AzVmImageListOffersOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--publisher")] string Publisher
) : AzOptions
{
    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }
}