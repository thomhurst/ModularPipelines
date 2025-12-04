using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("consumption", "budget", "create")]
public record AzConsumptionBudgetCreateOptions(
[property: CliOption("--amount")] string Amount,
[property: CliOption("--budget-name")] string BudgetName,
[property: CliOption("--category")] string Category,
[property: CliOption("--end-date")] string EndDate,
[property: CliOption("--start-date")] string StartDate,
[property: CliOption("--time-grain")] string TimeGrain
) : AzOptions
{
    [CliOption("--meter-filter")]
    public string? MeterFilter { get; set; }

    [CliOption("--resource-filter")]
    public string? ResourceFilter { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--resource-group-filter")]
    public string? ResourceGroupFilter { get; set; }
}