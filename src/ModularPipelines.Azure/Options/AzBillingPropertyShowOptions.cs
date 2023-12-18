using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "property", "show")]
public record AzBillingPropertyShowOptions : AzOptions
{
    [CommandSwitch("--cost-center")]
    public string? CostCenter { get; set; }
}