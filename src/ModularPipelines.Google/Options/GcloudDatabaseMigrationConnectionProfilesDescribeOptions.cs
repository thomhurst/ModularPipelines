using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "connection-profiles", "describe")]
public record GcloudDatabaseMigrationConnectionProfilesDescribeOptions(
[property: PositionalArgument] string ConnectionProfile,
[property: PositionalArgument] string Region
) : GcloudOptions;