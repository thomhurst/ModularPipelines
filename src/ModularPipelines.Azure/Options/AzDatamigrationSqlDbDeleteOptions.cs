using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "sql-db", "delete")]
public record AzDatamigrationSqlDbDeleteOptions : AzOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sqldb-instance-name")]
    public string? SqldbInstanceName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-db-name")]
    public string? TargetDbName { get; set; }

    [CliFlag("--yes")]
    public bool? Yes { get; set; }
}