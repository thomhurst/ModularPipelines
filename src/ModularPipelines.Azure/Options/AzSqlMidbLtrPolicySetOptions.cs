using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sql", "midb", "ltr-policy", "set")]
public record AzSqlMidbLtrPolicySetOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--managed-instance")]
    public string? ManagedInstance { get; set; }

    [CliOption("--monthly-retention")]
    public string? MonthlyRetention { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--week-of-year")]
    public string? WeekOfYear { get; set; }

    [CliOption("--weekly-retention")]
    public string? WeeklyRetention { get; set; }

    [CliOption("--yearly-retention")]
    public string? YearlyRetention { get; set; }
}