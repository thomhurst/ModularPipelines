using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("database-migration", "conversion-workspaces", "describe-ddls")]
public record GcloudDatabaseMigrationConversionWorkspacesDescribeDdlsOptions(
[property: CliArgument] string ConversionWorkspace,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--commit-id")]
    public string? CommitId { get; set; }

    [CliOption("--filter")]
    public string? Filter { get; set; }

    [CliOption("--tree-type")]
    public string? TreeType { get; set; }

    [CliFlag("--uncommitted")]
    public bool? Uncommitted { get; set; }
}