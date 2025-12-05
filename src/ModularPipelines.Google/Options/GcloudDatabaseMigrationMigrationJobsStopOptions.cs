using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "stop")]
public record GcloudDatabaseMigrationMigrationJobsStopOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region
) : GcloudOptions;