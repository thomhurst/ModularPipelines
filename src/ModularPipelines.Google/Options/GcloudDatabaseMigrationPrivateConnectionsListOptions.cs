using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "private-connections", "list")]
public record GcloudDatabaseMigrationPrivateConnectionsListOptions(
[property: CliOption("--region")] string Region
) : GcloudOptions;