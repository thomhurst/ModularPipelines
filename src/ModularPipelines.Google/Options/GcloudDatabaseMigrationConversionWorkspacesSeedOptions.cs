using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "seed")]
public record GcloudDatabaseMigrationConversionWorkspacesSeedOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--destination-connection-profile")] string DestinationConnectionProfile,
[property: CommandSwitch("--source-connection-profile")] string SourceConnectionProfile
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [BooleanCommandSwitch("--auto-commit")]
    public bool? AutoCommit { get; set; }
}