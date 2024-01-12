using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "start")]
public record GcloudDatabaseMigrationMigrationJobsStartOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--skip-validation")]
    public bool? SkipValidation { get; set; }
}