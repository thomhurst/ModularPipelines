using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "seed")]
public record GcloudDatabaseMigrationConversionWorkspacesSeedOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region,
[property: CliOption("--destination-connection-profile")] string DestinationConnectionProfile,
[property: CliOption("--source-connection-profile")] string SourceConnectionProfile
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliFlag("--auto-commit")]
    public bool? AutoCommit { get; set; }
}