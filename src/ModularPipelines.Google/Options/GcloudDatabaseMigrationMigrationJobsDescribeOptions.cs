using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "migration-jobs", "describe")]
public record GcloudDatabaseMigrationMigrationJobsDescribeOptions(
[property: CliArgument] string MigrationJob,
[property: CliArgument] string Region
) : GcloudOptions;