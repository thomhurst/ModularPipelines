using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("consumption", "budget", "update")]
public record AzConsumptionBudgetUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--amount")]
    public string? Amount { get; set; }

    [CommandSwitch("--budget-name")]
    public string? BudgetName { get; set; }

    [CommandSwitch("--category")]
    public string? Category { get; set; }

    [CommandSwitch("--e-tag")]
    public string? ETag { get; set; }

    [CommandSwitch("--filters")]
    public string? Filters { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--notifications")]
    public string? Notifications { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--time-grain")]
    public string? TimeGrain { get; set; }

    [CommandSwitch("--time-period")]
    public string? TimePeriod { get; set; }
}

