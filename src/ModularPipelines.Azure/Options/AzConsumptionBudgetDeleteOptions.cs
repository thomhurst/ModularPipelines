using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("consumption", "budget", "delete")]
public record AzConsumptionBudgetDeleteOptions(
[property: CliOption("--budget-name")] string BudgetName
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}