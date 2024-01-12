using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "restart")]
public record GcloudDatabaseMigrationMigrationJobsRestartOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--skip-validation")]
    public bool? SkipValidation { get; set; }
}