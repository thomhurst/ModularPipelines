using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "apply")]
public record GcloudDatabaseMigrationConversionWorkspacesApplyOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region,
[property: CliOption("--destination-connection-profile")] string DestinationConnectionProfile
) : GcloudOptions
{
    [CliFlag("--no-async")]
    public bool? NoAsync { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }
}