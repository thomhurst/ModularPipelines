using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "operations", "list")]
public record GcloudDatabaseMigrationOperationsListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;