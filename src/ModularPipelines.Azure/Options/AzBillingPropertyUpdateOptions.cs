using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "property", "update")]
public record AzBillingPropertyUpdateOptions : AzOptions
{
    [CommandSwitch("--cost-center")]
    public string? CostCenter { get; set; }
}