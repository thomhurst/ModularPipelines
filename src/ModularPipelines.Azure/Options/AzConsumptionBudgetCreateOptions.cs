using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "budget", "create")]
public record AzConsumptionBudgetCreateOptions(
[property: CommandSwitch("--amount")] string Amount,
[property: CommandSwitch("--budget-name")] string BudgetName,
[property: CommandSwitch("--category")] string Category,
[property: CommandSwitch("--end-date")] string EndDate,
[property: CommandSwitch("--start-date")] string StartDate,
[property: CommandSwitch("--time-grain")] string TimeGrain
) : AzOptions
{
    [CommandSwitch("--meter-filter")]
    public string? MeterFilter { get; set; }

    [CommandSwitch("--resource-filter")]
    public string? ResourceFilter { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--resource-group-filter")]
    public string? ResourceGroupFilter { get; set; }
}