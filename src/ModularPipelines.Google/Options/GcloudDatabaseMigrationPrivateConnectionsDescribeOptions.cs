using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "private-connections", "describe")]
public record GcloudDatabaseMigrationPrivateConnectionsDescribeOptions(
[property: PositionalArgument] string PrivateConnection,
[property: PositionalArgument] string Region
) : GcloudOptions;