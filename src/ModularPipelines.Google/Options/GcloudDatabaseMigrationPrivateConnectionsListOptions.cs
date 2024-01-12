using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "private-connections", "list")]
public record GcloudDatabaseMigrationPrivateConnectionsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;