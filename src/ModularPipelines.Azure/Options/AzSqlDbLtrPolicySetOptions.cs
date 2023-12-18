using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sql", "db", "ltr-policy", "set")]
public record AzSqlDbLtrPolicySetOptions : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--monthly-retention")]
    public string? MonthlyRetention { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--server")]
    public string? Server { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--week-of-year")]
    public string? WeekOfYear { get; set; }

    [CommandSwitch("--weekly-retention")]
    public string? WeeklyRetention { get; set; }

    [CommandSwitch("--yearly-retention")]
    public string? YearlyRetention { get; set; }
}

