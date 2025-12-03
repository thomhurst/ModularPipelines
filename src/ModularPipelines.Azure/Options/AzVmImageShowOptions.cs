using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vm", "image", "show")]
public record AzVmImageShowOptions : AzOptions
{
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

    [CliOption("--urn")]
    public string? Urn { get; set; }

    [CliOption("--version")]
    public string? Version { get; set; }
}