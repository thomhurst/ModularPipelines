using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "restart")]
public record GcloudDatabaseMigrationMigrationJobsRestartOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--skip-validation")]
    public bool? SkipValidation { get; set; }
}