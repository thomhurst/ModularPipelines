using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("postgres", "flexible-server", "migration", "create")]
public record AzPostgresFlexibleServerMigrationCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--properties")] string Properties,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--migration-mode")]
    public string? MigrationMode { get; set; }

    [CliOption("--migration-name")]
    public string? MigrationName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}