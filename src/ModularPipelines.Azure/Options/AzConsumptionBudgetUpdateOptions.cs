using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("consumption", "budget", "update")]
public record AzConsumptionBudgetUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

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

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--notifications")]
    public string? Notifications { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--time-grain")]
    public string? TimeGrain { get; set; }

    [CliOption("--time-period")]
    public string? TimePeriod { get; set; }
}