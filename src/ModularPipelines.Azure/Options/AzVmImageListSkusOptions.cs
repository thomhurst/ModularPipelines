using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "image", "list-skus")]
public record AzVmImageListSkusOptions(
[property: CliOption("--location")] string Location,
[property: CliOption("--offer")] string Offer,
[property: CliOption("--publisher")] string Publisher
) : AzOptions
{
    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }
}