using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "create")]
public record GcloudDatabaseMigrationConversionWorkspacesCreateOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--destination-database-engine")] string DestinationDatabaseEngine,
[property: CommandSwitch("--destination-database-version")] string DestinationDatabaseVersion,
[property: CommandSwitch("--source-database-engine")] string SourceDatabaseEngine,
[property: CommandSwitch("--source-database-version")] string SourceDatabaseVersion
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--global-settings")]
    public IEnumerable<KeyValue>? GlobalSettings { get; set; }
}