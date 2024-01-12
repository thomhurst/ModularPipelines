using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "list")]
public record GcloudDatabaseMigrationConversionWorkspacesListOptions(
[property: CommandSwitch("--region")] string Region
) : GcloudOptions;