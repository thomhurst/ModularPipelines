using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "list")]
public record GcloudDatabaseMigrationMigrationJobsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;