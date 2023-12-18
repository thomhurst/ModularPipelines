using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "location", "checktrialavailability")]
public record AzVmwareLocationCheckTrialAvailabilityOptions(
    [property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--sku")]
    public string? Sku { get; set; }
}