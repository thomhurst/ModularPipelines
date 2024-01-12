using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("database-migration", "conversion-workspaces", "delete")]
public record GcloudDatabaseMigrationConversionWorkspacesDeleteOptions(
[property: PositionalArgument] string ConversionWorkspace,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [BooleanCommandSwitch("--no-async")]
    public bool? NoAsync { get; set; }
}