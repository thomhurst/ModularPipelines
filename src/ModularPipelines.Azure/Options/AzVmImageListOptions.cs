using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vm", "image", "list")]
public record AzVmImageListOptions(
[property: CommandSwitch("--location")] string Location,
[property: CommandSwitch("--publisher")] string Publisher
) : AzOptions
{
    [BooleanCommandSwitch("--all")]
    public bool? All { get; set; }

    [CommandSwitch("--architecture")]
    public string? Architecture { get; set; }

    [CommandSwitch("--edge-zone")]
    public string? EdgeZone { get; set; }

    [CommandSwitch("--offer")]
    public string? Offer { get; set; }

    [CommandSwitch("--sku")]
    public string? Sku { get; set; }
}