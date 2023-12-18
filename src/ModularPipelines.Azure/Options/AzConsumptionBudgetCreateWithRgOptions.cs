using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "budget", "create-with-rg")]
public record AzConsumptionBudgetCreateWithRgOptions(
[property: CommandSwitch("--budget-name")] string BudgetName
) : AzOptions
{
    [CommandSwitch("--amount")]
    public string? Amount { get; set; }

    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--e-tag")]
    public string? ETag { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--notifications")]
    public string? Notifications { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--time-grain")]
    public string? TimeGrain { get; set; }

    [CommandSwitch("--time-period")]
    public string? TimePeriod { get; set; }
}