using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "apply")]
public record GcloudDatabaseMigrationConversionWorkspacesApplyOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region,
[property: CommandSwitch("--destination-connection-profile")] string DestinationConnectionProfile
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}