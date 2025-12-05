using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "operations", "list")]
public record GcloudDatabaseMigrationOperationsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;