using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("billing", "property", "update")]
public record AzBillingPropertyUpdateOptions : AzOptions
{
    [CliOption("--cost-center")]
    public string? CostCenter { get; set; }
}