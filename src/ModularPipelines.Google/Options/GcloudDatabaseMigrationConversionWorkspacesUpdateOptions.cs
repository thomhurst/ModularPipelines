using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "update")]
public record GcloudDatabaseMigrationConversionWorkspacesUpdateOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }
}