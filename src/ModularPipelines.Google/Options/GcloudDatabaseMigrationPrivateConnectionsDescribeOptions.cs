using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "private-connections", "describe")]
public record GcloudDatabaseMigrationPrivateConnectionsDescribeOptions(
[property: CliArgument] string PrivateConnection,
[property: CliArgument] string Region
) : GcloudOptions;