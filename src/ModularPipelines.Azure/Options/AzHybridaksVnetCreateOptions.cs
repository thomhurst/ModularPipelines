using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("hybridaks", "vnet", "create")]
public record AzHybridaksVnetCreateOptions(
[property: CliOption("--custom-location")] string CustomLocation,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--aods-vnet-id")]
    public string? AodsVnetId { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--moc-group")]
    public string? MocGroup { get; set; }

    [CliOption("--moc-location")]
    public string? MocLocation { get; set; }

    [CliOption("--moc-vnet-name")]
    public string? MocVnetName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--vsphere-segment-name")]
    public string? VsphereSegmentName { get; set; }
}