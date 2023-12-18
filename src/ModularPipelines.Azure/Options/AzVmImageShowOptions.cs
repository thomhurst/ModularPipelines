using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "show")]
public record AzVmImageShowOptions : AzOptions
{
    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--offer")]
    public string? Offer { get; set; }

    [CommandSwitch("--publisher")]
    public string? Publisher { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }

    [CommandSwitch("--urn")]
    public string? Urn { get; set; }

    [CommandSwitch("--version")]
    public string? Version { get; set; }
}