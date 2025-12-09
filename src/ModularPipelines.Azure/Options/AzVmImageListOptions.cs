using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vm", "image", "list")]
public record AzVmImageListOptions : AzOptions
{
    [CliFlag("--all")]
    public bool? All { get; set; }

    [CliOption("--architecture")]
    public string? Architecture { get; set; }

    [CliOption("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--offer")]
    public string? Offer { get; set; }

    [CliOption("--publisher")]
    public string? Publisher { get; set; }

    [CliOption("--sku")]
    public string? Sku { get; set; }
}