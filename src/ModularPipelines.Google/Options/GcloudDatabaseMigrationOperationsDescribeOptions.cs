using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "operations", "describe")]
public record GcloudDatabaseMigrationOperationsDescribeOptions(
[property: CliArgument] string Operation,
[property: CliArgument] string Region
) : GcloudOptions;