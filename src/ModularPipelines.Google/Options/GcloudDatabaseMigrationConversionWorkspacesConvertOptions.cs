using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "convert")]
public record GcloudDatabaseMigrationConversionWorkspacesConvertOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliFlag("--auto-commit")]
    public bool? AutoCommit { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }
}