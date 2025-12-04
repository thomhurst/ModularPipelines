using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "sql-db", "create")]
public record AzDatamigrationSqlDbCreateOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sqldb-instance-name")] string SqldbInstanceName,
[property: CliOption("--target-db-name")] string TargetDbName
) : AzOptions
{
    [CliOption("--migration-service")]
    public string? MigrationService { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--source-database-name")]
    public string? SourceDatabaseName { get; set; }

    [CliOption("--source-sql-connection")]
    public string? SourceSqlConnection { get; set; }

    [CliOption("--table-list")]
    public string? TableList { get; set; }

    [CliOption("--target-db-collation")]
    public string? TargetDbCollation { get; set; }

    [CliOption("--target-sql-connection")]
    public string? TargetSqlConnection { get; set; }
}