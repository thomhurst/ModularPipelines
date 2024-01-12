using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "migration-jobs", "delete")]
public record GcloudDatabaseMigrationMigrationJobsDeleteOptions(
[property: PositionalArgument] string MigrationJob,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--force")]
    public bool? Force { get; set; }
}