using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("functionapp", "list-consumption-locations")]
public record AzFunctionappListConsumptionLocationsOptions : AzOptions
{
    [CommandSwitch("--os")]
    public string? Os { get; set; }
}