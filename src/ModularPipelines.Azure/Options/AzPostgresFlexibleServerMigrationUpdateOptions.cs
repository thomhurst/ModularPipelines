using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "migration", "update")]
public record AzPostgresFlexibleServerMigrationUpdateOptions(
[property: CliOption("--migration-name")] string MigrationName
) : AzOptions
{
    [CliOption("--cancel")]
    public string? Cancel { get; set; }

    [CliOption("--cutover")]
    public string? Cutover { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--setup-replication")]
    public string? SetupReplication { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}