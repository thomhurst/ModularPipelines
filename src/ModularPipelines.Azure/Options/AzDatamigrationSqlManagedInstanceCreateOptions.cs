using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "sql-managed-instance", "create")]
public record AzDatamigrationSqlManagedInstanceCreateOptions(
[property: CliOption("--managed-instance-name")] string ManagedInstanceName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--target-db-name")] string TargetDbName
) : AzOptions
{
    [CliOption("--migration-service")]
    public string? MigrationService { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--offline-configuration")]
    public string? OfflineConfiguration { get; set; }

    [CliOption("--scope")]
    public string? Scope { get; set; }

    [CliOption("--source-database-name")]
    public string? SourceDatabaseName { get; set; }

    [CliOption("--source-location")]
    public string? SourceLocation { get; set; }

    [CliOption("--source-sql-connection")]
    public string? SourceSqlConnection { get; set; }

    [CliOption("--target-db-collation")]
    public string? TargetDbCollation { get; set; }

    [CliOption("--target-location")]
    public string? TargetLocation { get; set; }
}