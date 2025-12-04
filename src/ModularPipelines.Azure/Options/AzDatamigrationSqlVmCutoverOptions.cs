using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datamigration", "sql-vm", "cutover")]
public record AzDatamigrationSqlVmCutoverOptions(
[property: CliOption("--migration-operation-id")] string MigrationOperationId
) : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--sql-vm-name")]
    public string? SqlVmName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--target-db-name")]
    public string? TargetDbName { get; set; }
}