using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "convert")]
public record GcloudDatabaseMigrationConversionWorkspacesConvertOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }

    [BooleanCommandSwitch("--auto-commit")]
    public bool? AutoCommit { get; set; }

    [CommandSwitch("--filter")]
    public string? Filter { get; set; }
}