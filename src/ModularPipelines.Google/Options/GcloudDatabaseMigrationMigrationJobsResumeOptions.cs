using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "resume")]
public record GcloudDatabaseMigrationMigrationJobsResumeOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region
) : GcloudOptions;