using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "connection-profiles", "describe")]
public record GcloudDatabaseMigrationConnectionProfilesDescribeOptions(
[property: CliArgument] string ConnectionProfile,
[property: CliArgument] string Region
) : GcloudOptions;