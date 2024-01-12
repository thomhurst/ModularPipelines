using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "resume")]
public record GcloudDatabaseMigrationMigrationJobsResumeOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region
) : GcloudOptions;