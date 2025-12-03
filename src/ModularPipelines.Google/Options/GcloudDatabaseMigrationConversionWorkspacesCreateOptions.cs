using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "create")]
public record GcloudDatabaseMigrationConversionWorkspacesCreateOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region,
[property: CliOption("--destination-database-engine")] string DestinationDatabaseEngine,
[property: CliOption("--destination-database-version")] string DestinationDatabaseVersion,
[property: CliOption("--source-database-engine")] string SourceDatabaseEngine,
[property: CliOption("--source-database-version")] string SourceDatabaseVersion
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--global-settings")]
    public IEnumerable<KeyValue>? GlobalSettings { get; set; }
}