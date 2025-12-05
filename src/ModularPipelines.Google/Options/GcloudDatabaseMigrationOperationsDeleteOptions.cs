using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "operations", "delete")]
public record GcloudDatabaseMigrationOperationsDeleteOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;