using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datamigration", "sql-managed-instance", "show")]
public record AzDatamigrationSqlManagedInstanceShowOptions : AzOptions
{
    [CliOption("--expand")]
    public string? Expand { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--managed-instance-name")]
    public string? ManagedInstanceName { get; set; }

    [CliOption("--migration-operation-id")]
    public string? MigrationOperationId { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-db-name")]
    public string? TargetDbName { get; set; }
}