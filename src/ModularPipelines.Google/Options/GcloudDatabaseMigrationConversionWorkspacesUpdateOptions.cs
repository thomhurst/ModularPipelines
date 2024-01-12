using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "update")]
public record GcloudDatabaseMigrationConversionWorkspacesUpdateOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }
}