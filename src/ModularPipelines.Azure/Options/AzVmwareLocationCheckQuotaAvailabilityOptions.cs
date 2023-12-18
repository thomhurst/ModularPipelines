using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "location", "checkquotaavailability")]
public record AzVmwareLocationCheckquotaavailabilityOptions(
[property: CommandSwitch("--location")] string Location
) : AzOptions
{
    [CommandSwitch("--sku")]
    public string? Sku { get; set; }
}