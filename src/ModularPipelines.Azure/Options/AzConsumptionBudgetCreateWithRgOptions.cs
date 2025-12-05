using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("consumption", "budget", "create-with-rg")]
public record AzConsumptionBudgetCreateWithRgOptions : AzOptions
{
    [CliOption("--amount")]
    public string? Amount { get; set; }

    [CliOption("--budget-name")]
    public string? BudgetName { get; set; }

    [CliOption("--category")]
    public string? Category { get; set; }

    [CliOption("--e-tag")]
    public string? ETag { get; set; }

    [CliOption("--filters")]
    public string? Filters { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--notifications")]
    public string? Notifications { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--time-grain")]
    public string? TimeGrain { get; set; }

    [CliOption("--time-period")]
    public string? TimePeriod { get; set; }
}