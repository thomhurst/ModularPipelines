using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "delete")]
public record GcloudDatabaseMigrationMigrationJobsDeleteOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--force")]
    public bool? Force { get; set; }
}