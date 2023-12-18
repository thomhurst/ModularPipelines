using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("billing", "property", "update")]
public record AzBillingPropertyUpdateOptions : AzOptions
{
    [CommandSwitch("--cost-center")]
    public string? CostCenter { get; set; }
}