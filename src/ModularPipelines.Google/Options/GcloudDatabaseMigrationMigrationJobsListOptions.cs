using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "list")]
public record GcloudDatabaseMigrationMigrationJobsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;