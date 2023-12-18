using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "midb", "ltr-policy", "set")]
public record AzSqlMidbLtrPolicySetOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CommandSwitch("--monthly-retention")]
    public string? MonthlyRetention { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--week-of-year")]
    public string? WeekOfYear { get; set; }

    [CommandSwitch("--weekly-retention")]
    public string? WeeklyRetention { get; set; }

    [CommandSwitch("--yearly-retention")]
    public string? YearlyRetention { get; set; }
}